using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Models.Responses
{
    public record OrderRs
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long TotalAmount { get; set; }
        public int TotalUnits { get; set; }
        public OrderStatusEnum OrderStatusId { get; set; }
        public string OrderStatusDescription { get; set; }

        public long RayanFundOrderId { get; set; }

        /// <summary>
        /// سامانه درخواست کننده
        /// </summary>
        public string AppName { get; set; }
        public long CustomerId { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerNationalCode { get; set; }
        public string CustomerAccountSheba { get; set; }
        public string CustomerAccountNumber { get; set; }
        public int CustomerAccountBankId { get; set; }
        public string CustomerAccountBankName { get; set; }
        public int FundId { get; set; }
        public string FundName { get; set; }
        public long? SagaTransactionId { get; set; }

        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedById { get; set; }

    }
}
