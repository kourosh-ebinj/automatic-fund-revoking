using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses;
using Core.Abstractions.Caching;

namespace Application.Services.Abstractions.Caching
{
    public interface IBankAccountCacheService : IBaseHCacheService<BankAccountRs>
    {
        Task<BankAccountRs> GetById(int fundId, int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<BankAccountRs>> GetAll(int fundId, bool? isEnabled = null, CancellationToken cancellationToken = default);
    }
}
