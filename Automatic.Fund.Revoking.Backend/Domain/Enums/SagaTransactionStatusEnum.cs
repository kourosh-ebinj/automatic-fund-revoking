using System;
using System.ComponentModel;

namespace Domain.Enums
{
    public enum SagaTransactionStatusEnum : byte
    {
        [Description("لغو شده است")]
        Cancelled = 0,

        [Description("در انتظار تایید شدن وضعیت در رایان")]
        WaitingToGetMarkedAsConfirmed = 1,

        [Description("سفارش ابطال در رایان تایید شده است")]
        MarkedAsConfirmed = 2,

        [Description("در انتظار پرداخت ")]
        WaitingToGetPaid = 3,

        [Description("سفارش ابطال پرداخت شده است.")]
        Paid = 4,

        [Description("در انتظار بازگشت دادن تراکنش")]
        WaitingToGetReversed = 5,

        [Description("تراکنش ابطال قابل انجام نیست و برگشت خورده است.")]
        ReversedToDraft = 6,

        [Description("بازگرداندن تراکنش به وضعیت اولیه امکانپذیر نیست.")]
        FailedToReverseTransaction = 7,
    }
}
