using System;
using Application.Enums;
using Domain.Entities;

namespace Application.Models.Requests
{
    public record BankPayaPaymentRq
    {
        public BankAccount SourceBankAccount { get; set; }
        public string SourceAccountSheba { get; set; }
        public string DestAccountNumber { get; set; }
        public string DestAccountSheba { get; set; }
        public string DestNationalCode { get; set; }
        public string DestFullName { get; set; }
        public string SourceFullName { get; set; }
        public double Amount { get; set; }
        public string SourceComment { get; set; }
        public string DestComment { get; set; }
        public string PaymentId { get; set; }

        public PasargadBankPayaDetailTypeEnum DetailType { get; set; }

    }
}
