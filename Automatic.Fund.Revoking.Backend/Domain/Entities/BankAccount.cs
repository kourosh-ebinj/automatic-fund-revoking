using System;
using System.Collections.Generic;
using Domain.Abstractions;
using Core.DomainValidation.Helpers;

namespace Domain.Entities
{
    public class BankAccount: EntityBase, IValidate, ITrackable, IsSoftDeletable
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string ShebaNumber { get; set; }
        public int BankId { get; set; }
        public int FundId { get; set; }
        public double Balance { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }

        public Fund Fund{ get; set; }
        public Bank Bank { get; set; }
        public virtual ICollection<BankPayment> BankPayments { get; set; }

        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedById { get; set; }

        public void Validate() 
        {
            #region AccountNumber

            if (string.IsNullOrWhiteSpace(AccountNumber))
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(AccountNumber)));

            //if (Name.Length < 6)
            //    _ValidationErrors.Add(ErrorsHelper.MinLengthError(nameof(Name)));

            #endregion

            #region ShebaNumber

            if (string.IsNullOrWhiteSpace(ShebaNumber))
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(ShebaNumber)));

            #endregion

            #region BankId

            if (BankId < 1)
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(BankId)));

            #endregion

            #region FundId

            if (FundId < 1)
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(FundId)));

            #endregion


        }
    }
}
