using System;
using Domain.Entities;

namespace Application.Models.Requests
{
    public record BankInternalPaymentRq
    {
        public BankAccount SourceBankAccount { get; set; }
        public string DestAccountNumber { get; set; }
        public double Amount { get; set; }
        public string SourceComment { get; set; }
        public string DestComment { get; set; }

    }
}
