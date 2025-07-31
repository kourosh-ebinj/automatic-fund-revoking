using System;
using Core.Extensions;
using Domain.Enums;

namespace Application.Models.Responses
{
    public record SagaTransactionRs
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string Description { get; set; }
        public SagaTransactionStatusEnum Status { get; set; }
        public string StatusDescription => Status.ToDescription();

    }
}
