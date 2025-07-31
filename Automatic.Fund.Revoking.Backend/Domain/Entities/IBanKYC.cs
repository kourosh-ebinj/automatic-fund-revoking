using System;
using System.Collections.Generic;
using Domain.Abstractions;
using Core.DomainValidation.Helpers;

namespace Domain.Entities
{
    public class IBanKYC : EntityBase, IValidate
    {
        public int Id { get; set; }
        public string IBan { get; set; }
        public bool IsKYCCompliant { get; set; }


        public void Validate()
        {
            #region Title

            if (string.IsNullOrWhiteSpace(IBan))
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(IBan)));
            
            if (IBan.Length < 26 || IBan.Length > 26)
                _ValidationErrors.Add(ErrorsHelper.MinLengthError(nameof(IBan)));

            #endregion
        }
    }
}
