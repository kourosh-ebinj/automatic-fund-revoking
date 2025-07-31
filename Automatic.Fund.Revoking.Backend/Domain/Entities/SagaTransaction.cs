using System;
using System.Collections.Generic;
using Domain.Abstractions;
using Core.DomainValidation.Helpers;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities
{
    public class SagaTransaction : EntityBase, ITrackable
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string Description { get; set; }
        public SagaTransactionStatusEnum Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedById { get; set; }

        public virtual Order Order { get; set; }
    }
}
