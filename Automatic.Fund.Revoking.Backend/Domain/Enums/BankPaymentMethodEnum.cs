using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum BankPaymentMethodEnum : byte
    {
        [Description("داخلی")]
        Internal = 1,

        [Description("پایا")]
        Paya = 2,

        [Description("ساتنا")]
        Satna = 3,
    }
}
