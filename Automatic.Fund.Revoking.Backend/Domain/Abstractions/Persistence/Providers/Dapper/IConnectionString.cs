using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions.Persistence.Providers.Dapper
{
    public interface IConnectionString
    {
        public string ConnectionString { get; }

    }
}
