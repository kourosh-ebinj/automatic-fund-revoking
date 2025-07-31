using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum TransactionStatusEnum : byte
    {
        Failed = 0,
        Aborted = 1,
        Successfull = 2,
    }
}
