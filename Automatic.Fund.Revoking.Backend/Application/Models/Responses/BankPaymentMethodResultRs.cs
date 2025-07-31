using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Enums;
using Domain.Enums;

namespace Application.Models.Responses
{
    public record BankPaymentMethodResultRs
    {
        public string TransactionId { get; set; }
        public string TransactionDate { get; set; }
        public string Message { get; set; }
        public string MessageCode { get; set; }
        public TransactionStatusEnum PaymentStatus { get; set; }
    }
}
