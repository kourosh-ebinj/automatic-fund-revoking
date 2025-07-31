using System;
using System.Collections.Generic;
using Core.DomainValidation.Models;

namespace Core.Abstractions.Entities
{
    public interface IValidate
    {
        void Validate();

        IEnumerable<DomainValidationError> ValidationErrors { get; }
    }
}
