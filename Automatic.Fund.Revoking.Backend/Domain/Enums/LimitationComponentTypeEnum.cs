using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum LimitationComponentTypeEnum: byte
    {
        BankAccountBalance = 1,
        FundCancellationMaxUnits = 2,
        FundCancellationMinUnits = 3,
        FundCancellationMinAmount = 4,
        FundCancellationMaxAmount = 5,
        CustomerWhitelist = 6,
        AppWhitelist = 7,

    }
}
