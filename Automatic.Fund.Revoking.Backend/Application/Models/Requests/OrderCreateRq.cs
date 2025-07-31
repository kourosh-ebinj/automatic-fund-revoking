using System;

namespace Application.Models.Requests
{
    public record OrderCreateRq
    {
        public string Title { get; set; }
        public long? TotalAmount { get; set; }
        public int TotalUnits { get; set; }
        public string AppName { get; set; }
        public string AppCode { get; set; }
        public long BackOfficeOrderId { get; set; }

        public long CustomerId { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerNationalCode { get; set; }
        public string CustomerAccountSheba { get; set; }
        public string CustomerAccountNumber { get; set; }
        public int? CustomerAccountBankId { get; set; }       
        public long RayanFundOrderId { get; set; }

    }

}
