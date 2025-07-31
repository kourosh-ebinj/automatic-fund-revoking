using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class ReverseOrderToDraftConsumer : BaseConsumer<ReverseOrderToDraftRq>, IReverseOrderToDraftConsumer
    {
        private readonly ISagaTransactionService _sagaTransactionService;
        private readonly IOrderService _orderService;
        private readonly IFundService _fundService;
        private readonly IRayanService _rayanService;
        private readonly ISagaTransactionService _SagaTransactionService;

        public ReverseOrderToDraftConsumer(IServiceProvider provider)
        {
            _sagaTransactionService = provider.GetService<ISagaTransactionService>();
            _orderService = provider.GetRequiredService<IOrderService>();
            _fundService = provider.GetRequiredService<IFundService>();
            _rayanService = provider.GetRequiredService<IRayanService>();
            _SagaTransactionService = provider.GetRequiredService<ISagaTransactionService>();

        }

        public async Task Consume(ConsumeContext<ReverseOrderToDraftRq> context)
        {
            const string reason = "بازگرداندن وضعیت به حالت اولیه در سیستم رایان انجام نشد.";
            var message = context.Message;

            var order = await _orderService.GetById(message.FundId, message.OrderId);
            _guard.Assert(order is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $"سفارش ({message.OrderId}) یافت نشد.");

            var fund = await _fundService.GetById(message.FundId);
            _guard.Assert(fund is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $"صندوق ({message.FundId}) یافت نشد.");

            var sagaTransaction = await _sagaTransactionService.GetByOrderIdWithLock(message.OrderId,
                                            SagaTransactionStatusEnum.WaitingToGetReversed, context.CancellationToken);
            _guard.Assert(sagaTransaction is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $" تراکنش ساگای سفارش ({message.OrderId}) یافت نشد.");

            var stateChanged = await _rayanService.ReverseOrderToDraft(fund.DsCode, order.RayanFundOrderId, context.CancellationToken);

            order.OrderStatusId = OrderStatusEnum.UnSettled;
            if (stateChanged)
                order.OrderStatusDescription = $"تراکنش ابطال سفارش {order.Id} با موفقیت رولبک شد. ";
            else
            {
                var title = $"امکان رولبک تراکنش سفارش {order.Id} وجود ندارد. ";
                order.OrderStatusDescription = title + reason;

                sagaTransaction.Status = SagaTransactionStatusEnum.FailedToReverseTransaction;
                await _SagaTransactionService.Update(sagaTransaction);
            }

            await _orderService.UpdateOrderStatus(order.FundId, order.Id, order.OrderStatusId, order.OrderStatusDescription, true, context.CancellationToken);

            await base.Consume(context);
        }

    }
}
