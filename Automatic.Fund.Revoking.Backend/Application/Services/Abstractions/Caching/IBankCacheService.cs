using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses;
using Core.Abstractions.Caching;

namespace Application.Services.Abstractions.Caching
{
    public interface IBankCacheService : IBaseSCacheService<IEnumerable<BankRs>>
    {
        Task<IEnumerable<BankRs>> GetAll(int fundId, bool? isEnabled = null);
        Task<BankRs> GetById(int fundId, int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<BankRs>> GetActiveBanks(int fundId, CancellationToken cancellationToken = default);
        Task<BankRs> GetActiveBankById(int fundId, int bankId, CancellationToken cancellationToken = default);

    }
}
