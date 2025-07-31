using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Responses.ThirdParties;
using Domain.Enums;

namespace Application.Models.Responses
{
    public record CustomerInfoRs
    {
        public long CustomerId { get; set; }
        public bool IsLegal { get; set; }
        public long CustomerStatusId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string MobileNumber { get; set; }
        public string FullName { get; set; }

        public CustomerBankAccountRs BankAccount { get; set; }
    }

    public record CustomerBankAccountRs
    {
        public long BankAccountId { get; set; }
        public string AccountNumber { get; set; }
        public string BaTypeName { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string shabaNumber { get; set; }

    }
}
