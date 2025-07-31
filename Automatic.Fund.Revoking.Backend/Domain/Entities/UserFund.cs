using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;
using Core.Attributes;
using Core.DomainValidation.Helpers;

namespace Domain.Entities
{
    public class UserFund : EntityBase, IValidate
    {
        public int Id { get; set; }
        public long UserId{ get; set; }
        public int FundId { get; set; }
        
        public virtual Fund Fund{ get; set; }

        public void Validate()
        {
            
            #region UserId

            if (UserId < 1)
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(UserId)));

            if (FundId < 1)
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(FundId)));

            #endregion
        }
    }
}
