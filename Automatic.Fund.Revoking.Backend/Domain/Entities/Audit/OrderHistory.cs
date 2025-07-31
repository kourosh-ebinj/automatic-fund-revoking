using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;
using Core.Attributes;
using Core.DomainValidation.Helpers;
using Domain.Enums;

namespace Domain.Entities.Audit
{
    public class OrderHistory : EntityBase
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public DateTime ValidFrom { get; set; }

        public string Title { get; set; }
        public double TotalAmount { get; set; }
        public int TotalUnits { get; set; }
        public OrderStatusEnum OrderStatusId { get; set; }
        public string OrderStatusDescription { get; set; }
        public long BackOfficeOrderId { get; set; }
        public string AppName { get; set; }
        public long CustomerId { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerNationalCode { get; set; }
        public string CustomerAccountSheba { get; set; }
        public string CustomerAccountNumber { get; set; }
        public int CustomerAccountBankId { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }


        public virtual Order Order { get; set; }
    }
}
