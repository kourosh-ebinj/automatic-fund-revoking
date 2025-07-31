using System;
using System.Collections.Generic;
using Domain.Abstractions;
using Core.DomainValidation.Helpers;

namespace Domain.Entities
{
    public class Bank : EntityBase, IValidate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BackOfficeBankId { get; set; }
        public string ProviderClassName { get; set; }
        public bool IsEnabled { get; set; }
        public int FundId { get; set; }

        public virtual Fund Fund { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<BankPayment> BankPayments { get; set; }

        public void Validate()
        {
            #region Title

            if (string.IsNullOrWhiteSpace(Name))
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(Name)));

            if (Name.Length < 6)
                _ValidationErrors.Add(ErrorsHelper.MinLengthError(nameof(Name)));

            if (FundId > 0)
                _ValidationErrors.Add(ErrorsHelper.MinLengthError(nameof(FundId)));

            #endregion
        }
    }
}
