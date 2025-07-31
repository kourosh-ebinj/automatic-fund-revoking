using System;
using System.Threading.Tasks;
using Application.Models.Requests.Messaging;
using Application.Models.Responses;
using Application.Services.Abstractions;
using Application.Services.Abstractions.ThirdParties.Banks;
using Core.Abstractions.Messaging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Application.Enums;
using Application.Models.Requests;
using Domain.Enums;
using Domain.Entities;
using Application.Services.Abstractions.Persistence;
using Core.Extensions;
using Microsoft.Extensions.Logging;
using Application;
using Application.Services.Messaging.Consumers;
using Core.Services.Messaging;

namespace Infrastructure.Services.Messaging.Consumers
{
    public class PayOrderConsumer : BaseConsumer<PayOrderRq>, IPayOrderConsumer
    {
        const string TransactionCommentFarsiPart = "*نیتروپی* پرداخت داخلی از حساب";

        private readonly ISagaTransactionService _sagaTransactionService;
        private readonly IOrderService _orderService;
        private readonly IBankingProviderSuggestService _bankingProviderSuggestService;
        private readonly IBankingProviderFactoryService _bankingProviderFactoryService;
        private readonly IBankPaymentService _bankPaymentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendEndpointProvider _sendEndpoint;

        public PayOrderConsumer(IServiceProvider provider)
        {
            _sagaTransactionService = provider.GetService<ISagaTransactionService>();
            _orderService = provider.GetRequiredService<IOrderService>();
            _bankingProviderSuggestService = provider.GetRequiredService<IBankingProviderSuggestService>();
            _bankingProviderFactoryService = provider.GetRequiredService<IBankingProviderFactoryService>();
            _bankPaymentService = provider.GetRequiredService<IBankPaymentService>();
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            _sendEndpoint = provider.GetRequiredService<ISendEndpointProvider>();
        }

        public async Task Consume(ConsumeContext<PayOrderRq> context)
        {
            var message = context.Message;

            var order = await _orderService.GetById(message.FundId, message.OrderId);
            _guard.Assert(order is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $"سفارش ({message.OrderId}) یافت نشد.");
            if (order.OrderStatusId != OrderStatusEnum.Accepted)
                return;

            var sagaTransaction = await _sagaTransactionService.GetByOrderIdWithLock(message.OrderId, SagaTransactionStatusEnum.MarkedAsConfirmed);
            _guard.Assert(sagaTransaction is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $" تراکنش ساگای سفارش ({message.OrderId}) یافت نشد.");

            var destinationBankId = message.CustomerAccountBankId;
            var paymentSuggestionModel = await _bankingProviderSuggestService.GetBestSuggestion(message.FundId, message.OrderId, message.TotalAmount, destinationBankId, context.CancellationToken);
            var bankProviderInstance = await _bankingProviderFactoryService.InstantiateBankingProvider(order.FundId, paymentSuggestionModel.SourceBankAccount.BankId, context.CancellationToken);

            try
            {
                BankPaymentMethodResultRs paymentResult = null;
                var transactionComment = $"{TransactionCommentFarsiPart} {paymentSuggestionModel.SourceBankAccount.AccountNumber} به حساب {order.CustomerAccountNumber} ";
                switch (paymentSuggestionModel.BankPaymentMethod)
                {
                    case BankPaymentMethodEnum.Internal:
                        paymentResult = await bankProviderInstance.InternalPayment(new BankInternalPaymentRq()
                        {
                            Amount = order.TotalAmount,
                            SourceBankAccount = paymentSuggestionModel.SourceBankAccount,
                            DestAccountNumber = order.CustomerAccountNumber,
                            DestComment = transactionComment,
                            SourceComment = transactionComment,
                        }, context.CancellationToken);
                        break;
                    case BankPaymentMethodEnum.Paya:
                        paymentResult = await bankProviderInstance.PayaPayment(new BankPayaPaymentRq()
                        {
                            Amount = order.TotalAmount,
                            SourceFullName = order.CustomerFullName,
                            SourceBankAccount = paymentSuggestionModel.SourceBankAccount,
                            SourceAccountSheba = paymentSuggestionModel.SourceBankAccount.ShebaNumber,
                            SourceComment = "",
                            DestAccountSheba = order.CustomerAccountSheba,
                            DestAccountNumber = order.CustomerAccountNumber,
                            DestFullName = order.CustomerFullName,
                            DestComment = $"*نیتروپی* پرداخت پایا از شبا {paymentSuggestionModel.SourceBankAccount.ShebaNumber} به شبا {order.CustomerAccountSheba}",
                            DetailType = PasargadBankPayaDetailTypeEnum.ModiriateNaghdinegi,
                            DestNationalCode = order.CustomerNationalCode,
                        }, context.CancellationToken);
                        break;
                    case BankPaymentMethodEnum.Satna:
                        paymentResult = await bankProviderInstance.SatnaPayment(new BankSatnaPaymentRq()
                        {
                            Amount = order.TotalAmount,
                            SourceFullName = order.CustomerFullName,
                            SourceBankAccount= paymentSuggestionModel.SourceBankAccount,
                            SourceAccountSheba = paymentSuggestionModel.SourceBankAccount.ShebaNumber,
                            DestAccountSheba = order.CustomerAccountSheba,
                            DestAccountNumber = order.CustomerAccountNumber,
                            DestFullName = order.CustomerFullName,
                            Description = $"*نیتروپی* پرداخت ساتنا از شبا {paymentSuggestionModel.SourceBankAccount.ShebaNumber} به شبا {order.CustomerAccountSheba}",
                            DetailType = PasargadBankPayaDetailTypeEnum.ModiriateNaghdinegi,
                            DestNationalCode = order.CustomerNationalCode,
                        }, context.CancellationToken);
                        break;
                    default:
                        throw new NotImplementedException("Payment method is not supported.");
                }

                if (paymentResult.PaymentStatus == TransactionStatusEnum.Successfull)
                {
                    order.OrderStatusId = OrderStatusEnum.Settled;
                    order.OrderStatusDescription = "ابطال با موفقیت انجام شد.";
                }
                else
                {
                    order.OrderStatusId = OrderStatusEnum.UnSettled;
                    order.OrderStatusDescription = $"ابطال سفارش ({order.Id}) امکان پذیر نیست. تغییر وضعیت در سیستم رایان انجام نشد.";
                }

                await _bankPaymentService.Create(new BankPayment()
                {
                    BankUniqueId = paymentResult.TransactionId,
                    TransactionStatusId = paymentResult.PaymentStatus,
                    SourceBankAccountId = paymentSuggestionModel.SourceBankAccount.Id,
                    DestinationShebaNumber = order.CustomerAccountSheba,
                    DestinationBankId = destinationBankId,
                    TotalAmount = order.TotalAmount,
                    OrderId = order.Id,
                    Description = paymentResult.Message,
                    BankPaymentMethodId = paymentSuggestionModel.BankPaymentMethod,
                }, context.CancellationToken);
                await _orderService.UpdateOrderStatus(order.FundId, order.Id, order.OrderStatusId, order.OrderStatusDescription, false, context.CancellationToken);
                await _unitOfWork.SaveChanges(context.CancellationToken);

                if (paymentResult.PaymentStatus == TransactionStatusEnum.Successfull)
                {
                    sagaTransaction.Status = SagaTransactionStatusEnum.Paid;
                    await _sagaTransactionService.Update(sagaTransaction, SagaTransactionStatusEnum.MarkedAsConfirmed);
                }
                else
                {
                    var endpoint = await _sendEndpoint.GetSendEndpoint(new Uri($"queue:{Constants.Queue_ReverseOrderToDraft}"));
                    await endpoint.Send(new ReverseOrderToDraftRq()
                    {
                        OrderId = message.OrderId,
                        FundId = order.FundId,
                        SenderAppName = message.SenderAppName,
                    });

                    sagaTransaction.Status = SagaTransactionStatusEnum.WaitingToGetReversed;
                    sagaTransaction.Description = "امکان پرداخت وجود ندارد. تراکنش ساگا برگشت خواهد خورد.";
                    await _sagaTransactionService.Update(sagaTransaction, SagaTransactionStatusEnum.MarkedAsConfirmed);
                    return;
                }

            }
            catch (Exception ex)
            {
                // Very important
                // Exception should not happen 
                _logger.LogCritical(ex, $"Payment consumer encounted with a problem. OrderId ({order.Id})");
                throw;
            }

            await base.Consume(context);
        }

    }
}
