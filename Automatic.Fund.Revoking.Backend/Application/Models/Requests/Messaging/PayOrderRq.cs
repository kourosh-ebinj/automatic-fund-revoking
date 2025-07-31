using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Enums;
using Application.Models.Responses;
using Core.Models.Messaging;

namespace Application.Models.Requests.Messaging
{
    public record PayOrderRq : BaseConsumerModel
    {
        public long OrderId { get; set; }
        public int FundId { get; set; }
        public int CustomerAccountBankId { get; set; }
        public double TotalAmount { get; set; }

    }
}
