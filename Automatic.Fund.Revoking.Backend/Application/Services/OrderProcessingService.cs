using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums;
using Application.Models.Requests;
using Application.Models.Responses;
using Application.Services.Abstractions;
using Application.Services.Abstractions.ThirdParties;
using Core.Exceptions;
using Core.Extensions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Core.Abstractions;
using Application.Services.Abstractions.Persistence;
using MassTransit;
using Application.Models.Requests.Messaging;
using Application.Models;

namespace Application.Services
{
    public class OrderProcessingService : IOrderProcessingService
    {

        private readonly ISagaTransactionService _sagaTransactionService;
        private readonly ILimitationService _limitationService;
        private readonly ICustomerService _customerService;
        private readonly IBankService _bankService;
        private readonly IOrderService _orderService;
        private readonly IRayanService _rayanService;
        private readonly IFundService _fundService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGuard _guard;
        private readonly ILogger<OrderProcessingService> _logger;
        private readonly ISendEndpointProvider _sendEndpoint;
        protected readonly ApplicationSettingExtenderModel _applicationSetting;

        public OrderProcessingService(ISagaTransactionService sagaTransactionService,
                                      IOrderService orderService,
                                      IRayanService rayanService,
                                      ILimitationService limitationService,
                                      ICustomerService customerService,
                                      IFundService fundService,
                                      IBankService bankService,
                                      IUnitOfWork unitOfWork,
                                      IGuard guard,
                                      ISendEndpointProvider sendEndpoint,
                                      ApplicationSettingExtenderModel applicationSetting,
                                      ILogger<OrderProcessingService> logger)
        {
            _sagaTransactionService = sagaTransactionService;
            _orderService = orderService;
            _rayanService = rayanService;
            _customerService = customerService;
            _limitationService = limitationService;
            _fundService = fundService;
            _bankService = bankService;
            _unitOfWork = unitOfWork;
            _guard = guard;
            _sendEndpoint = sendEndpoint;
            _applicationSetting = applicationSetting;
            _logger = logger;

        }

        public async Task<IEnumerable<Order>> ImportRayanCancelledFundsToOrders(int fundId, DateTime startDate, DateTime endDate,
                                                                                CancellationToken cancellationtoken = default)
        {
            try
            {

                var fund = await _fundService.GetActiveFundById(fundId);
                _guard.Assert(fund is not null, Core.Enums.ExceptionCodeEnum.BadRequest, "شناسه صندوق اشتباه است");

                var cancelledFunds = await _rayanService.GetCancelledFunds(fund.Id, startDate, endDate);
                if (!cancelledFunds.Any())
                    return Enumerable.Empty<Order>();

                var rayanFundOrderIds = cancelledFunds.Select(e => e.Id);
                var ordersQuery = _orderService.GetQuery().Where(e => rayanFundOrderIds.Contains(e.RayanFundOrderId) && !e.IsDeleted);
                var newRayanFundOrderIds = rayanFundOrderIds.Except(await ordersQuery.Select(e => e.RayanFundOrderId).ToListAsync());

                if (!newRayanFundOrderIds.Any())
                    return Enumerable.Empty<Order>();

                var newRayanOrders = cancelledFunds.Where(e => newRayanFundOrderIds.Contains(e.Id));

                var customerInfoTasks = newRayanOrders.Select(async e =>
                    await _rayanService.GetCustomerInfo(fund.DsCode, e.NationalCode.Remove("-"))
                );
                var customerInfos = await Task.WhenAll(customerInfoTasks);
                customerInfos = customerInfos.Where(e => e is not null).ToArray();
                var bankIdsDic = await GetBankIds(fundId, customerInfos.Where(e => e.BankAccount is not null).Select(e => e.BankAccount.BankId), cancellationtoken);
                var customerIds = await GetCustomerIds(fundId, newRayanOrders.Select(e => e.CustomerId), cancellationtoken);

                var missingCustomerIds = newRayanOrders.Select(e => e.CustomerId).Except(customerIds.Keys);
                if (missingCustomerIds.Any())
                    newRayanOrders = newRayanOrders.ExceptBy(missingCustomerIds, e => e.CustomerId);
                var orderRequests = newRayanOrders.AsParallel().Select(rayanOrder =>
                {
                    var customerInfo = customerInfos.FirstOrDefault(e => e.CustomerId == rayanOrder.CustomerId);
                    if (customerInfo is null)
                    {
                        _logger.LogWarning($"customerinfo not found for {rayanOrder.CustomerName}, nationalCode ({rayanOrder.NationalCode}), CustomerId ({rayanOrder.CustomerId})");
                        return null;
                    }

                    var rayanBankId = customerInfo?.BankAccount?.BankId;
                    int? bankId = null;
                    if (rayanBankId.HasValue)
                        if (bankIdsDic.TryGetValue(rayanBankId.Value, out var _bankId))
                            bankId = _bankId;

                    return new OrderCreateRq()
                    {
                        Title = GetOrderTitle(rayanOrder),
                        TotalUnits = rayanOrder.FundUnit,
                        TotalAmount = GetOrderTotalAmount(rayanOrder),
                        AppName = rayanOrder.AppuserName,
                        AppCode = rayanOrder.UserName,
                        BackOfficeOrderId = rayanOrder.FundOrderId,
                        CustomerAccountNumber = rayanOrder.customerAccountNumber,
                        CustomerAccountSheba = customerInfo?.BankAccount?.shabaNumber,
                        CustomerFullName = rayanOrder.CustomerName,
                        CustomerNationalCode = rayanOrder.NationalCode.Remove("-"),
                        CustomerId = customerIds[rayanOrder.CustomerId],
                        CustomerAccountBankId = bankId,
                        RayanFundOrderId = rayanOrder.Id,
                    };
                });

                var orders = await _orderService.CreateBatch(fundId, orderRequests.Where(e => e is not null).ToList(), false, cancellationtoken);

                foreach (var order in orders)
                {
                    try
                    {
                        LimitationComponentValidatorResultRs validationResult = null;
                        validationResult = await _limitationService.Validate(LimitationTypeEnum.PreOrdering, fundId, order);
                        order.OrderStatusId = GetOrderStatus(validationResult.ValidatorResultStatus);
                        order.OrderStatusDescription = validationResult.StatusMessage;
                    }
                    catch (CustomException validationEx)
                    {
                        var msg = $"Limitation Validation failed for OrderId: {order.Id}. Error: {validationEx.Message}";
                        _logger.LogInformation(msg, order);
                    }
                    await _orderService.AddToHistory(order, cancellationtoken);
                }

                await _unitOfWork.SaveChanges(cancellationToken: cancellationtoken);

                return orders;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Order>> PayAcceptedOrders(int fundId, CancellationToken cancellationToken = default)
        {
            var orders = await _orderService.GetAllQueryable(fundId, OrderStatusEnum.Accepted)
                                    .Include(e => e.Customer).ThenInclude(e => e.Fund)
                                    .AsTracking()
                                    .ToListAsync(cancellationToken);

            foreach (var order in orders)
            {
                LimitationComponentValidatorResultRs validationResult = null;
                validationResult = await _limitationService.Validate(LimitationTypeEnum.PrePayment, fundId, order);
                if (validationResult.ValidatorResultStatus is not LimitationComponentValidatorResultEnum.Accepted)
                    continue;

                var sagaTransaction = await _sagaTransactionService.GetByOrderIdOrDefault(order.Id);
                if (sagaTransaction == null)
                {
                    var endpoint = await _sendEndpoint.GetSendEndpoint(new Uri($"queue:{Constants.Queue_MarkOrderAsConfirmed}"));
                    await endpoint.Send(new MarkOrderAsConfirmedRq()
                    {
                        OrderId = order.Id,
                        FundId = order.Customer.Fund.Id,
                        SenderAppName = _applicationSetting.App.Name,
                    }, cancellationToken);
                    await _sagaTransactionService.Create(new SagaTransactionCreateRq()
                    {
                        Status = SagaTransactionStatusEnum.WaitingToGetMarkedAsConfirmed,
                        OrderId = order.Id,
                    });
                }
            }
            return orders;
        }

        private async Task<IDictionary<int, int>> GetBankIds(int fundId, IEnumerable<int> backOfficeBankIds, CancellationToken cancellationToken)
        {
            if (!backOfficeBankIds.Any()) return new Dictionary<int, int>();

            var banks = await _bankService.GetAllQueryable(fundId).
                                          Select(e => new { e.Id, e.BackOfficeBankId }).
                                          Where(e => backOfficeBankIds.Contains(e.BackOfficeBankId)).
                                          ToListAsync(cancellationToken);

            return banks.DistinctBy(e => e.BackOfficeBankId).
                         ToDictionary(e => e.BackOfficeBankId, e => e.Id);
        }

        private string GetOrderTitle(CancelledFundRs rayanOrder) =>
            $"ابطال شماره {rayanOrder.FundOrderId}";

        private long GetOrderTotalAmount(CancelledFundRs rayanOrder) =>
            rayanOrder.FundUnit * _applicationSetting.App.FaceValue;

        private OrderStatusEnum GetOrderStatus(LimitationComponentValidatorResultEnum validationStatus) =>
            validationStatus switch
            {
                LimitationComponentValidatorResultEnum.Rejected => OrderStatusEnum.Rejected,
                LimitationComponentValidatorResultEnum.NeedsApproval => OrderStatusEnum.NeedsApproval,
                LimitationComponentValidatorResultEnum.Accepted => OrderStatusEnum.Accepted,
                _ => throw new CustomException(Core.Enums.ExceptionCodeEnum.BadRequest, $"{nameof(validationStatus)} is wrong."),
            };

        private async Task<IDictionary<long, long>> GetCustomerIds(int fundId, IEnumerable<long> rayanCustomerIds, CancellationToken cancellationToken = default)
        {
            var customers = await _customerService.GetQueryableByBackOfficeId(fundId, backOfficeIds: rayanCustomerIds, cancellationToken: cancellationToken)
                                    .Select(e => new { e.Id, e.BackOfficeId })
                                    .ToDictionaryAsync(a => a.BackOfficeId, b => b.Id);

            return customers;
        }

    }
}
