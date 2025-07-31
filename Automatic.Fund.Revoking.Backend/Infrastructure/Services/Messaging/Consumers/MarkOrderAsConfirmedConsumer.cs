using System;
using System.Threading.Tasks;
using Application;
using Application.Models.Requests.Messaging;
using Application.Services.Abstractions;
using Application.Services.Abstractions.ThirdParties;
using Application.Services.Messaging.Consumers;
using Core.Abstractions.Messaging;
using Core.Extensions;
using Core.Services.Messaging;
using Domain.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Messaging.Consumers
{
    public class MarkOrderAsConfirmedConsumer : BaseConsumer<MarkOrderAsConfirmedRq>, IMarkOrderAsConfirmedConsumer
    {
        private readonly ISagaTransactionService _sagaTransactionService;
        private readonly IOrderService _orderService;
        private readonly IFundService _fundService;
        private readonly IRayanService _rayanService;
        private readonly ISendEndpointProvider _sendEndpoint;

        public MarkOrderAsConfirmedConsumer(IServiceProvider provider)
        {
            _sagaTransactionService = provider.GetService<ISagaTransactionService>();
            _orderService = provider.GetRequiredService<IOrderService>();
            _fundService = provider.GetRequiredService<IFundService>();
            _rayanService = provider.GetRequiredService<IRayanService>();
            _sendEndpoint = provider.GetRequiredService<ISendEndpointProvider>();

        }

        public async Task Consume(ConsumeContext<MarkOrderAsConfirmedRq> context)
        {
            var message = context.Message;
            try
            {
                var order = await _orderService.GetById(message.FundId, message.OrderId);
                _guard.Assert(order is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $"سفارش ({message.OrderId}) یافت نشد.");
                if (order.OrderStatusId != OrderStatusEnum.Accepted)
                    return;

                var fund = await _fundService.GetById(message.FundId);
                _guard.Assert(fund is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $"صندوق ({message.FundId}) یافت نشد.");

                var sagaTransaction = await _sagaTransactionService.GetByOrderIdWithLock(message.OrderId,
                            SagaTransactionStatusEnum.WaitingToGetMarkedAsConfirmed, context.CancellationToken);
                _guard.Assert(sagaTransaction is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $" تراکنش ساگای سفارش ({message.OrderId}) یافت نشد.");

                var stateChanged = await _rayanService.MarkOrderAsConfirmed(fund.DsCode, order.RayanFundOrderId, context.CancellationToken);
                if (!stateChanged)
                {
                    order.OrderStatusId = OrderStatusEnum.UnSettled;
                    var description = $"ابطال سفارش ({order.Id}) امکان پذیر نیست. تغییر وضعیت در سیستم رایان انجام نشد.";
                    await _orderService.UpdateOrderStatus(message.FundId, order.Id, OrderStatusEnum.UnSettled, description, true, context.CancellationToken);

                    sagaTransaction.Status = SagaTransactionStatusEnum.Cancelled;
                    sagaTransaction.Description = description;
                    await _sagaTransactionService.Update(sagaTransaction, SagaTransactionStatusEnum.WaitingToGetMarkedAsConfirmed);
                    
                    return;
                }

                var endpoint = await _sendEndpoint.GetSendEndpoint(new Uri($"queue:{Constants.Queue_PayOrder}"));
                await endpoint.Send(new PayOrderRq()
                {
                    OrderId = message.OrderId,
                    FundId = message.FundId,
                    TotalAmount = order.TotalAmount,
                    SenderAppName = message.SenderAppName,
                    CustomerAccountBankId = order.CustomerAccountBankId,
                });

                sagaTransaction.Status = SagaTransactionStatusEnum.MarkedAsConfirmed;
                await _sagaTransactionService.Update(sagaTransaction, SagaTransactionStatusEnum.WaitingToGetMarkedAsConfirmed);

                await base.Consume(context);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
