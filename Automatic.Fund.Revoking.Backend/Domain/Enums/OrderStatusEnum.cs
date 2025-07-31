using System;
using System.ComponentModel;

namespace Domain.Enums
{
    public enum OrderStatusEnum : byte
    {
        [Description("برگشت خورده")]
        Rejected = 0,

        [Description("در انتظار تایید")]
        NeedsApproval = 1,
        
        [Description("در انتظار پرداخت")]
        Accepted = 2,

        [Description("با موفقیت ابطال شده است")]
        Settled = 3,

        [Description("ابطال انجام نشده است")]
        UnSettled = 4,

        //[Description("پرداخت شده")]
        //Paid = 3,

        //[Description("پرداخت نا موفق")]
        //UnPaid = 5,

    }
}
