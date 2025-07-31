using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Enums;
using Core.Models.Messaging;

namespace Application.Models.Requests.Messaging
{
    public record MarkOrderAsConfirmedRq : BaseConsumerModel
    {
        public long OrderId { get; set; }
        public int FundId { get; set; }
        public string SenderAppName { get; set; }
    }
}
