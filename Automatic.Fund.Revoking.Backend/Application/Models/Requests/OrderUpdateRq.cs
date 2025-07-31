using System;
using Domain.Enums;

namespace Application.Models.Requests
{
    public record OrderUpdateRq
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public OrderStatusEnum OrderStatusId { get; set; }
        public string OrderStatusDescription { get; set; }
        //public long? TotalAmount { get; set; }
        //public int TotalUnits { get; set; }
        //public string AppName { get; set; }
        //public string AppCode { get; set; }
        //public long BackOfficeOrderId { get; set; }

        //public long CustomerId { get; set; }
        //public string CustomerFullName { get; set; }
        //public string CustomerNationalCode { get; set; }
        //public string CustomerAccountSheba { get; set; }
        //public string CustomerAccountNumber { get; set; }
        //public int? CustomerAccountBankId { get; set; }
        //public int FundId { get; set; }
        //public long RayanFundOrderId { get; set; }

    }

}
