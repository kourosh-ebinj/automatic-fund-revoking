using System;
using System.Collections.Generic;
using Domain.Abstractions;
using Core.DomainValidation.Helpers;
using Domain.Entities.ThirdParties;

namespace Domain.Entities
{
    public class Fund : EntityBase, IValidate, IsSoftDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DsCode { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Limitation> Limitations { get; set; }
        public virtual ICollection<UserFund> UserFunds { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Bank> Banks { get; set; }
        public virtual ICollection<RayanCustomer> RayanCustomers{ get; set; }
        public virtual ICollection<RayanFundOrder> RayanFundOrders { get; set; }

        public void Validate()
        {
            #region Name

            if (string.IsNullOrWhiteSpace(Name))
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(Name)));

            #endregion

            #region DsCode

            if (DsCode < 1)
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(DsCode)));

            #endregion
        }
    }
}
