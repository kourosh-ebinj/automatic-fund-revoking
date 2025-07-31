using System;

namespace Application.Models.Responses
{
    public record BankAccountRs
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string ShebaNumber { get; set; }
        public int BankId { get; set; }
        public int FundId { get; set; }
        public bool IsEnabled { get; set; }
    }
}
