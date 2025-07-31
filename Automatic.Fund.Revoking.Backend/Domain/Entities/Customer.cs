using System;
using System.Collections.Generic;
using Domain.Abstractions;
using Core.DomainValidation.Helpers;

namespace Domain.Entities
{
    public class Customer : EntityBase, IValidate, ITrackable
    {
        public long Id { get; set; }
        public long BackOfficeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string TradingCode { get; set; }
        public string MobileNumber { get; set; }
        public int FundId { get; set; }
        
        public virtual Fund Fund { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedById { get; set; }

        public void Validate()
        {

            if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
            {
                var propertyName = $"{nameof(FirstName)} and {nameof(LastName)}";
                _ValidationErrors.Add(ErrorsHelper.EmptyError(propertyName));
            }

            //if (string.IsNullOrWhiteSpace(NationalCode))
            //    _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(NationalCode)));

            //if (string.IsNullOrWhiteSpace(MobileNumber))
            //    _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(MobileNumber)));

        }
    }
}
