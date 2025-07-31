using System;
using Domain.Abstractions;
using Core.DomainValidation.Helpers;

namespace Domain.Entities
{
    public class CustomerBankAccount: EntityBase, IValidate, ITrackable
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string ShebaNumber { get; set; }
        public int BankId { get; set; }
        public long CustomerId { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedById { get; set; }

        public Customer Customer { get; set; }

        public void Validate() 
        {
            #region AccountNumber

            if (string.IsNullOrWhiteSpace(AccountNumber))
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(AccountNumber)));

            #endregion

            #region ShebaNumber

            if (string.IsNullOrWhiteSpace(ShebaNumber))
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(ShebaNumber)));

            #endregion

            #region BankId

            if (BankId < 1)
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(BankId)));

            #endregion

            #region CustomerId

            if (CustomerId < 1)
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(CustomerId)));

            #endregion


        }
    }
}
