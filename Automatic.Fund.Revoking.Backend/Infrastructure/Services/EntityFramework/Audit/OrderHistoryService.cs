using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses;
using Application.Models.Responses.Reports;
using Application.Services.Abstractions.Audit;
using Application.Services.Abstractions.Persistence;
using Core.Models.Responses;
using Core.Models;
using Domain.Entities.Audit;
using Application.Extensions;
using Core.Constants;
using Core.Abstractions;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.EntityFramework.Audit
{

    public class OrderHistoryService : CRUDService<OrderHistory>, IOrderHistoryService
    {
        public IExcelBuilderService _excelBuilderService { get; set; }

        public OrderHistoryService(IUnitOfWork unitOfWork, IExcelBuilderService excelBuilderService) : base(unitOfWork)
        {
            _excelBuilderService = excelBuilderService;

        }

        public async Task<OrderHistoryRs> GetById(int fundId, long id, CancellationToken cancellationToken = default)
        {
            var fund = await GetByIdInternal(fundId, id);

            return _mapper.Map<OrderHistoryRs>(fund);
        }

        public async Task<PaginatedList<OrderHistoryRs>> GetAll(int fundId, long orderId, int? pageSize = null, int? pageNumber = null, string orderBy = "",
                                                              CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = $"createdat desc";

            var query = GetAllQueryableByOrderId(fundId, orderId);

            var list = await query.ToPaginatedList(pageSize ?? GlobalConstants.DefaultPageSize, pageNumber ?? 1, orderBy, cancellationToken);

            var histories = _mapper.Map<ICollection<OrderHistoryRs>>(list.Items);

            return new PaginatedList<OrderHistoryRs>()
            {
                PageNumber = list.PageNumber,
                Items = histories,
                PageSize = list.PageSize,
                TotalItems = list.TotalItems,
                TotalPages = list.TotalPages,
            };
        }

        public IQueryable<OrderHistory> GetAllQueryableByOrderId(int fundId, long  orderId)
        {
            var query = GetAllQueryable(fundId)
                .Where(e => e.OrderId == orderId);

            return query;
        }

        public IQueryable<OrderHistory> GetAllQueryable(int fundId)
        {
            var query = GetQuery()
                .Include(e => e.Order).ThenInclude(e => e.Customer)
                .Where(e => e.Order.Customer.FundId == fundId);

            return query;
        }

        //public async Task<OrderHistory> Create(OrderHistory entity, CancellationToken cancellationToken = default)
        //{
        //    var orderHistory = await base.Create(entity);

        //    return orderHistory;
        //}

        public async Task<PaginatedList<OrderHistoryReportRs>> Report(int fundId, long orderId, int? pageSize = null, int? pageNumber = null, string orderBy = "", 
                                                                      CancellationToken cancellationToken = default)
        {
            var histories = await GetAll( fundId, orderId, pageSize, pageNumber, orderBy, cancellationToken);

            var items = histories.Items.Select(orderHistory => new OrderHistoryReportRs()
            {
                AppName = orderHistory.AppName,
                BackOfficeOrderId = orderHistory.BackOfficeOrderId,
                CreatedById = orderHistory.CreatedById,
                CreatedAt = orderHistory.CreatedAt,
                CustomerAccountBankId = orderHistory.CustomerAccountBankId,
                CustomerAccountBankName = orderHistory.CustomerAccountBankName,
                CustomerAccountNumber = orderHistory.CustomerAccountNumber,
                CustomerAccountSheba = orderHistory.CustomerAccountSheba,
                CustomerFullName = orderHistory.CustomerFullName,
                CustomerId = orderHistory.CustomerId,
                CustomerNationalCode = orderHistory.CustomerNationalCode,
                FundName = orderHistory.FundName,
                Id = orderHistory.Id,
                ModifiedById = orderHistory.ModifiedById,
                ModifiedDate = orderHistory.ModifiedAt,
                OrderStatusId = orderHistory.OrderStatusId,
                OrderStatusDescription = orderHistory.OrderStatusDescription,
                Title = orderHistory.Title,
                TotalAmount = orderHistory.TotalAmount,
                TotalUnits = orderHistory.TotalUnits,
            }).ToImmutableList();

            return new PaginatedList<OrderHistoryReportRs>
            {
                Items = items,
                PageNumber = histories.PageNumber,
                PageSize = histories.PageSize,
                TotalItems = histories.TotalItems,
                TotalPages = histories.TotalPages,
            };
        }

        public async Task<ExcelResultRs> Excel(int fundId, long orderId, CancellationToken cancellationToken = default)
        {
            var sheetName = $" تاریخچه سفارش شماره {orderId}";

            var histories = await Report(fundId, orderId, pageSize: int.MaxValue, cancellationToken: cancellationToken);
            return new ExcelResultRs()
            {
                Bytes = await _excelBuilderService.ExportToExcel(histories.Items.ToOrderHistoryReportRs(), sheetName),
                Title = sheetName,
            };
        }


        private async Task<OrderHistory> GetByIdInternal(int fundId, long id, CancellationToken cancellationToken = default)
        {
            var history = await GetAllQueryable(fundId)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            //var history = await Find(id, cancellationToken);

            return history;
        }

    }
}
