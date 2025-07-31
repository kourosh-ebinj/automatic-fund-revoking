using System;
using Domain.Enums;

namespace Application.Models.Responses.Reports
{
    public record BankPaymentReportRs
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string SourceBankAccountNumber { get; set; }
        public string SourceBankName { get; set; }
        public string DestinationBankName { get; set; }
        public string DestinationShebaNumber { get; set; }
        public int DestinationBankId { get; set; }
        public double TotalAmount { get; set; }
        public string BankUniqueId { get; set; }
        public string Description { get; set; }
        public BankPaymentMethodEnum BankPaymentMethodId { get; set; }
        public TransactionStatusEnum TransactionStatusId { get; set; }

        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedById { get; set; }

    }
}
