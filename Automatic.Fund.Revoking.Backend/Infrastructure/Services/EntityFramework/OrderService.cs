using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Extensions;
using Application.Models.Requests;
using Application.Models.Responses;
using Application.Models.Responses.Reports;
using Application.Services.Abstractions;
using Application.Services.Abstractions.Audit;
using Application.Services.Abstractions.Persistence;
using Core.Abstractions;
using Core.Constants;
using Core.Enums;
using Core.Models;
using Core.Models.Responses;
using Domain.Entities;
using Domain.Entities.Audit;
using Domain.Enums;
using Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.EntityFramework
{
    /// <summary>
    /// Order service 
    /// </summary>
    public class OrderService : CRUDService<Order>, IOrderService
    {
        private readonly IBankService _bankService;
        public IExcelBuilderService _excelBuilderService { get; set; }
        private readonly IOrderHistoryService _orderHistoryService;

        public OrderService(IBankService bankService, IOrderHistoryService orderHistoryService,
                            IExcelBuilderService excelBuilderService,
                            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _bankService = bankService;
            _orderHistoryService = orderHistoryService;
            _excelBuilderService = excelBuilderService;
        }

        public async Task<PaginatedList<OrderRs>> GetAll(int fundId, OrderStatusEnum? status = null,
                                                         int? pageSize = null, int? pageNumber = null, string orderBy = "",
                                                         CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = $"createdat desc";

            var query = GetAllQueryable(fundId, status)
                            .Include(e => e.SagaTransaction)
                            .Include(e => e.CustomerAccountBank)
                            .Include(e => e.Customer).ThenInclude(e => e.Fund);
            var list = await query.ToPaginatedList(pageSize ?? GlobalConstants.DefaultPageSize, pageNumber ?? 1, orderBy, cancellationToken);

            var orders = _mapper.Map<ICollection<OrderRs>>(list.Items);

            return new PaginatedList<OrderRs>()
            {
                PageNumber = list.PageNumber,
                Items = orders,
                PageSize = list.PageSize,
                TotalItems = list.TotalItems,
                TotalPages = list.TotalPages,
            };
        }

        public async Task<OrderRs> GetById(int fundId, long id, CancellationToken cancellationToken = default)
        {
            var order = await GetAllQueryable(fundId)
                .Include(e => e.Customer).ThenInclude(e => e.Fund)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            var orderRs = _mapper.Map<OrderRs>(order);

            var bank = await _bankService.GetById(order.Customer.FundId, orderRs.CustomerAccountBankId);
            if (bank is not null)
                orderRs.CustomerAccountBankName = bank.Name;

            return orderRs;
        }

        public IQueryable<Order> GetAllQueryable(int fundId, OrderStatusEnum? status = null)
        {
            var query = GetQuery().Where(e => !e.IsDeleted && e.Customer.FundId == fundId);

            if (status is not null)
                query = query.Where(e => e.OrderStatusId == status);

            return query;
        }

        public async Task<Order> Create(int fundId, OrderCreateRq request, bool addToHistory = true, CancellationToken cancellationToken = default)
        {
            var order = _mapper.Map<Order>(request);

            order = await base.Create(order);

            if (addToHistory)
                await AddToHistory(order);

            return order;
        }

        public async Task<IEnumerable<Order>> CreateBatch(int fundId, IEnumerable<OrderCreateRq> request, bool addToHistory = true, CancellationToken cancellationToken = default)
        {
            try
            {

                var ordersTasks = request.Select(async e =>
                {
                    await ValidateToCreate(e);
                    return await Create(fundId, e, addToHistory);
                });
                var orders = await Task.WhenAll(ordersTasks);
                return orders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Order> Update(int fundId, OrderUpdateRq request, CancellationToken cancellationToken = default)
        {
            var order = await GetByIdInternal(fundId, request.Id, cancellationToken);
            order.OrderStatusId = request.OrderStatusId;
            order.OrderStatusDescription = request.OrderStatusDescription;

            _guard.Assert(order is not null, ExceptionCodeEnum.BadRequest, "سفارشی با این شناسه یافت نشد.");

            return await UpdateInternal(fundId, order, cancellationToken);
        }


        public async Task<IEnumerable<Order>> UpdateBatch(int fundId, IEnumerable<Order> entities, CancellationToken cancellationToken = default)
        {
            try
            {
                var ordersTasks = entities.Select(async order =>
                {
                    return await UpdateInternal(fundId, order, cancellationToken);
                });
                var orders = await Task.WhenAll(ordersTasks);
                return orders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<OrderRs> UpdateOrderStatus(int fundId, long orderId, OrderStatusEnum orderStatus, string orderStatusDescription, bool saveChanges = false,
                                                     CancellationToken cancellationToken = default)
        {
            _guard.Assert(orderId > 0, ExceptionCodeEnum.BadRequest, "شناسه سفارش معتبر نمی باشد.");
            _guard.Assert(orderStatus != OrderStatusEnum.Accepted ? !string.IsNullOrWhiteSpace(orderStatusDescription) : false,
                            ExceptionCodeEnum.BadRequest,
                            "وضعیت سفارش معتبر نمی باشد.");

            var currentOrder = await GetByIdInternal(fundId, orderId);
            _guard.Assert(currentOrder is not null, ExceptionCodeEnum.BadRequest, "سفارشی با این شناسه یافت نشد.");

            var newOrder = _mapper.Map<Order>(currentOrder);
            newOrder.OrderStatusId = orderStatus;
            newOrder.OrderStatusDescription = orderStatusDescription;

            await ValidateToUpdate(currentOrder, newOrder);
            await base.Update(newOrder);
            await AddToHistory(newOrder);

            if (saveChanges)
                await _unitOfWork.SaveChanges(cancellationToken);

            return _mapper.Map<OrderRs>(newOrder);
        }

        public async Task<OrderRs> SetAsAccepted(int fundId, long orderId, string orderStatusDescription, CancellationToken cancellationToken = default)
        {

            return await UpdateOrderStatus(fundId, orderId, OrderStatusEnum.Accepted, orderStatusDescription, true, cancellationToken);
        }

        public async Task<OrderRs> SetAsRejected(int fundId, long orderId, string orderStatusDescription, CancellationToken cancellationToken = default)
        {

            return await UpdateOrderStatus(fundId, orderId, OrderStatusEnum.Rejected, orderStatusDescription, true, cancellationToken);
        }

        public async Task Delete(int fundId, long id, CancellationToken cancellationToken = default)
        {
            var order = await GetAllQueryable(fundId).FirstOrDefaultAsync(x => x.Id == id);
            _guard.Assert(order is not null, ExceptionCodeEnum.BadRequest, "سفارشی با این شناسه یافت نشد.");

            ValidateToDelete(order);

            order.IsDeleted = true;
            await base.Update(order);
            await AddToHistory(order);

            await _unitOfWork.SaveChanges(cancellationToken);
        }

        public async Task<PaginatedList<OrderReportRs>> Report(int fundId, OrderStatusEnum? status = null, int? pageSize = null, int? pageNumber = null, string orderBy = "", CancellationToken cancellationToken = default)
        {
            var orders = await GetAll(fundId, status, pageSize, pageNumber, orderBy, cancellationToken);

            var items = orders.Items.Select(order => new OrderReportRs()
            {
                AppName = order.AppName,
                RayanFundOrderId = order.RayanFundOrderId,
                CreatedById = order.CreatedById,
                CreatedAt = order.CreatedAt,
                CustomerAccountBankId = order.CustomerAccountBankId,
                CustomerAccountBankName = order.CustomerAccountBankName,
                CustomerAccountNumber = order.CustomerAccountNumber,
                CustomerAccountSheba = order.CustomerAccountSheba,
                CustomerFullName = order.CustomerFullName,
                CustomerId = order.CustomerId,
                CustomerNationalCode = order.CustomerNationalCode,
                FundName = order.FundName,
                Id = order.Id,
                ModifiedById = order.ModifiedById,
                ModifiedDate = order.ModifiedAt,
                OrderStatusId = order.OrderStatusId,
                OrderStatusDescription = order.OrderStatusDescription,
                Title = order.Title,
                TotalAmount = order.TotalAmount,
                TotalUnits = order.TotalUnits,
            }).ToImmutableList();

            return new PaginatedList<OrderReportRs>
            {
                Items = items,
                PageNumber = orders.PageNumber,
                PageSize = orders.PageSize,
                TotalItems = orders.TotalItems,
                TotalPages = orders.TotalPages,
            };
        }

        public async Task<ExcelResultRs> Excel(int fundId, OrderStatusEnum? status = null, CancellationToken cancellationToken = default)
        {
            var sheetName = "ابطال های ثبت شده";

            var orders = await Report(fundId, status, pageSize: int.MaxValue, cancellationToken: cancellationToken);
            return new ExcelResultRs()
            {
                Bytes = await _excelBuilderService.ExportToExcel(orders.Items.ToOrderReportExcelRs(), sheetName),
                Title = sheetName,
            };
        }

        private async ValueTask ValidateToCreate(OrderCreateRq request)
        {
            _guard.Assert(!string.IsNullOrWhiteSpace(request.CustomerAccountSheba), ExceptionCodeEnum.BadRequest, $" شماره شبای مشتری نامعتبر است ({request.CustomerId}).");
            _guard.Assert(request.CustomerAccountBankId is not null, ExceptionCodeEnum.BadRequest, $"شناسه بانک مشتری نامعتبر است  ({request.CustomerId}).");
            _guard.Assert(!string.IsNullOrWhiteSpace(request.CustomerFullName), ExceptionCodeEnum.BadRequest, $"نام مشتری نامعتبر است ({request.CustomerId}).");
            _guard.Assert(!string.IsNullOrEmpty(request.CustomerNationalCode), ExceptionCodeEnum.BadRequest, $"کد ملی مشتری نامعتبر است  ({request.CustomerId}).");
            _guard.Assert(request.TotalAmount is not null, ExceptionCodeEnum.BadRequest, $"مبلغ سفارش نامعتبر است.");

            await Task.CompletedTask;
        }

        private async ValueTask ValidateToUpdate(Order currentOrder, Order newOrder)
        {

            await Task.CompletedTask;
        }

        private void ValidateToDelete(Order order)
        {

        }

        public async Task AddToHistory(Order order, CancellationToken cancellationToken = default)
        {
            var orderHistory = _mapper.Map<OrderHistory>(order);
            orderHistory.Order = order;
            orderHistory.CreatedAt = DateTime.Now;
            await _orderHistoryService.Create(orderHistory, cancellationToken);
        }

        private async Task<Order> GetByIdInternal(int fundId, long id, CancellationToken cancellationToken = default) =>
            await GetAllQueryable(fundId)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

        private async Task<Order> UpdateInternal(int fundId, Order order, CancellationToken cancellationToken = default)
        {
            _guard.Assert(order is not null, ExceptionCodeEnum.BadRequest, "سفارشی با این شناسه یافت نشد.");

            await ValidateToUpdate(order, order);

            await base.Update(order);
            await AddToHistory(order);

            return order;
        }

    }
}
