using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.Models.Requests
{
    public record SagaTransactionCreateRq
    {
        public long OrderId { get; set; }
        public string Description { get; set; }
        public SagaTransactionStatusEnum Status { get; set; }

    }

}
