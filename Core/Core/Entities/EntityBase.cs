using System;
using System.Collections.Generic;
using Core.DomainValidation.Models;

namespace Core.Entities
{
    public abstract class EntityBase  //<TId> where TId : struct
    {
        protected EntityBase() { }

        //public virtual TId Id { get; set; }
        //public abstract string TableName { get; }

        protected List<DomainValidationError> _ValidationErrors { get; set; } = new List<DomainValidationError>();
        public IEnumerable<DomainValidationError> ValidationErrors => _ValidationErrors;

    }
}
