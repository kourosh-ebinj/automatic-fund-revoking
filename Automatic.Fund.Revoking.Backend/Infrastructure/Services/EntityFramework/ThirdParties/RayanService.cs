using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Application.Enums;
using Application.Models.Responses;
using Application.Models.Responses.ThirdParties.Rayan;
using Application.Services.Abstractions;
using Application.Services.Abstractions.HttpClients.ThirdParties;
using Application.Services.Abstractions.Persistence;
using Application.Services.Abstractions.ThirdParties;
using AutoMapper;
using Core.Abstractions;
using Core.Extensions;
using Domain.Entities.ThirdParties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.EntityFramework.ThirdParties
{
    public class RayanService : IRayanService
    {

        private readonly IFundService _fundService;
        private readonly IRayanClientService _rayanClientService;
        private readonly IRayanCustomerService _rayanCustomerService;
        private readonly IRayanFundOrderService _rayanFundOrderService;
        private readonly IGuard _guard;
        private readonly IMapper _mapper;
        private readonly ILogger<RayanService> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public RayanService(IFundService fundService,
                            IRayanClientService rayanClientService,
                            IRayanCustomerService rayanCustomerService,
                            IUnitOfWork unitOfWork,
                            IRayanFundOrderService rayanFundOrderService,
                            IGuard guard,
                            IMapper mapper,
                            ILogger<RayanService> logger)
        {
            _fundService = fundService;
            _rayanClientService = rayanClientService;
            _rayanCustomerService = rayanCustomerService;
            _rayanFundOrderService = rayanFundOrderService;
            _guard = guard;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CancelledFundRs>> GetCancelledFunds(int fundId, DateTime startDate, DateTime endDate,
                                                                          CancellationToken cancellationToken = default)
        {

            var fund = await _fundService.GetById(fundId);
            _guard.Assert(fund is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $"fundId({fundId}) is not valid.");

            var orders = await _rayanClientService.GetFundRequests(fund.DsCode, startDate, endDate, pageSize: IRayanClientService.PAGESIZE, cancellationToken: cancellationToken);

            var cancelledItems = orders.Where(e => e.OrderType.Equals(Constants.OrderType_Canceled, StringComparison.InvariantCultureIgnoreCase) &&
                                                   e.foStatusId == 0); // 0 = پیش نویس


            return await SyncRayanOrders(fund.Id, cancelledItems, cancellationToken);
        }

        public async Task<PaginatedRayanCustomerRs> SyncRayanCustomers(FundRs fund, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var paginatedCustomersList = await _rayanClientService.GetCustomersList(fund.DsCode, pageNumber: pageNumber, pageSize: pageSize, cancellationToken: cancellationToken);

            var syncedRayanCustomers = await SyncRayanCustomers(fund.Id, paginatedCustomersList.Result, cancellationToken);

            return new PaginatedRayanCustomerRs()
            {
                Result = syncedRayanCustomers,
                PageNumber = paginatedCustomersList.PageNumber,
                PageSize = paginatedCustomersList.PageSize,
                TotalItems = paginatedCustomersList.Total,
            };
        }

        public async Task<CustomerInfoRs> GetCustomerInfo(int dsCode, string nationalCode, CancellationToken cancellationToken = default)
        {
            var rayanCustomerInfo = await _rayanClientService.GetCustomerInfo(dsCode, nationalCode, cancellationToken: cancellationToken);

            var customerInfo = _mapper.Map<CustomerInfoRs>(rayanCustomerInfo);

            if (customerInfo is null) return null;

            var defaultBankAaccount = rayanCustomerInfo.BankAccounts.FirstOrDefault(e => e.IsDefault == 1);
            if (defaultBankAaccount is not null)
                customerInfo.BankAccount = _mapper.Map<CustomerBankAccountRs>(defaultBankAaccount);

            return customerInfo;
        }

        public async Task<bool> MarkOrderAsConfirmed(int dsCode, long rayanFundOrderId, CancellationToken cancellationToken = default)
        {
            var fromStatus = RayanStatusEnum.DRAFT;
            var toStatus = RayanStatusEnum.WAIT;

            var rayanFundOrder = await _rayanFundOrderService.GetById(rayanFundOrderId);
            _guard.Assert(rayanFundOrder is not null, Core.Enums.ExceptionCodeEnum.BadRequest, "OrderId is not valid.");

            var statusChanged = await _rayanClientService.UpdateOrderStatus(dsCode, rayanFundOrder.FundOrderId, rayanFundOrder.OrderDate, fromStatus, toStatus, cancellationToken: cancellationToken);
            if (!statusChanged)
                return false;

            fromStatus = toStatus;
            toStatus = RayanStatusEnum.CONFIRMED;

            return await _rayanClientService.UpdateOrderStatus(dsCode, rayanFundOrder.FundOrderId, rayanFundOrder.OrderDate, fromStatus, toStatus, cancellationToken: cancellationToken);
        }

        public async Task<bool> ReverseOrderToDraft(int dsCode, long rayanFundOrderId, CancellationToken cancellationToken = default)
        {
            var fromStatus = RayanStatusEnum.CONFIRMED;
            var toStatus = RayanStatusEnum.WAIT;

            var rayanFundOrder = await _rayanFundOrderService.GetById(rayanFundOrderId);
            _guard.Assert(rayanFundOrder is not null, Core.Enums.ExceptionCodeEnum.BadRequest, "OrderId is not valid.");

            var statusChanged = await _rayanClientService.UpdateOrderStatus(dsCode, rayanFundOrder.FundOrderId, rayanFundOrder.OrderDate, fromStatus, toStatus, cancellationToken: cancellationToken);
            if (!statusChanged)
                return false;

            fromStatus = toStatus;
            toStatus = RayanStatusEnum.DRAFT;

            return await _rayanClientService.UpdateOrderStatus(dsCode, rayanFundOrder.FundOrderId, rayanFundOrder.OrderDate, fromStatus, toStatus, cancellationToken: cancellationToken);
        }

        private async Task<IEnumerable<CancelledFundRs>> SyncRayanOrders(int fundId, IEnumerable<RayanFundOrderRs> rayanOrders, CancellationToken cancellationToken)
        {
            try
            {
                var rayanFundOrderIds = rayanOrders.Select(e => e.FundOrderId);
                var ordersQuery = _rayanFundOrderService.GetAllQueryable().Where(e => rayanFundOrderIds.Contains(e.FundOrderId));
                var existingOrders = await ordersQuery.ToListAsync();
                var newRayanFundOrderIds = rayanFundOrderIds.Except(existingOrders.Select(e => e.FundOrderId));

                var newOrders = new List<RayanFundOrder>();
                if (newRayanFundOrderIds.Any())
                {
                    rayanOrders.AsParallel().ForAll(e => e.FundId = fundId);
                    newOrders.AddRange(await _rayanFundOrderService.CreateBatch(rayanOrders.Where(e => newRayanFundOrderIds.Contains(e.FundOrderId)),
                                                                       cancellationToken));
                }
                var result = newOrders.AsEnumerable().Union(existingOrders);

                return _mapper.Map<IEnumerable<CancelledFundRs>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Saving RayanFundOrder records failed. Request: {0}", rayanOrders.ToJsonString());
                throw;
            }
        }

        private async Task<IEnumerable<RayanCustomer>> SyncRayanCustomers(int fundId, IEnumerable<RayanCustomerRs> rayanCustomers, CancellationToken cancellationToken)
        {
            try
            {
                var rayanCustomerIds = rayanCustomers.Select(e => e.CustomerId);
                var ordersQuery = _rayanCustomerService.GetAllQueryable(fundId).Where(e => rayanCustomerIds.Contains(e.CustomerId));
                var existingCustomers = await ordersQuery.ToListAsync(cancellationToken);

                var newCustomerIds = rayanCustomers.Select(e => e.CustomerId).Except(existingCustomers.Select(e => e.CustomerId));
                var newCustomerInfos = rayanCustomers.Where(e => newCustomerIds.Contains(e.CustomerId));
                var newCustomers = _mapper.Map<IEnumerable<RayanCustomer>>(newCustomerInfos);

                existingCustomers.AsParallel().ForAll(e => e.FundId = fundId);
                newCustomers.AsParallel().ForAll(e => e.FundId = fundId);

                await _rayanCustomerService.Update(existingCustomers);
                await _rayanCustomerService.Create(newCustomers.ToImmutableList());

                await _unitOfWork.SaveChanges(cancellationToken);
                return existingCustomers.Union(newCustomers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Saving RayanCustomer records failed. ");
                throw;
            }
        }
    }
}
