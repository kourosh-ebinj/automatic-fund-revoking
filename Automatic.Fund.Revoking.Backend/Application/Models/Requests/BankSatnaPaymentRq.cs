using System;
using Application.Enums;
using Domain.Entities;

namespace Application.Models.Requests
{
    public record BankSatnaPaymentRq
    {
        public double Amount { get; set; }
        public string DestAccountNumber { get; set; }
        public string DestFullName { get; set; }
        public BankAccount SourceBankAccount { get; set; }
        public string SourceAccountSheba { get; set; }
        public string DestAccountSheba { get; set; }
        public string DestNationalCode { get; set; }
        public string SourceFullName { get; set; }
        public string Description { get; set; }

        public PasargadBankPayaDetailTypeEnum DetailType { get; set; }

    }
}
