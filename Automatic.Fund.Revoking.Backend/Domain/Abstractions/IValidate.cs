using System;
using System.Collections.Generic;
using Core.DomainValidation.Models;

namespace Domain.Abstractions
{
    public interface IValidate
    {
        void Validate();

        IEnumerable<DomainValidationError> ValidationErrors { get; }
    }
}
