using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public sealed class Constants
    {

        public const string CacheKey_BankAccountBalance = "BankAccountBalance";
        public const string CacheKey_RayanAuthenticateToken = "BankAccountBalance";
        public const string CacheKey_RayanBackOfficeAuthenticateToken = "BackOfficeBankAccountBalance";
        public const string OrderType_Canceled = "ابطال";

        public const string Queue_MarkOrderAsConfirmed = "markorderasconfirmed-queue";
        public const string Queue_ReverseOrderToDraft = "reverseordertodraft-queue";
        public const string Queue_PayOrder = "payorder-queue";

        public const string Role_Custodian = "Custodian";
        public const string Role_SystemAdmin = "SystemAdmin";
        public const string Role_FundsAdmin = "FundsAdmin";
        public const string Role_FundManager = "FundManager";
    }
}
