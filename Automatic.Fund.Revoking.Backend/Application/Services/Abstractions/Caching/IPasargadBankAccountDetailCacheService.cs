using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses;
using Application.Models.Responses.ThirdParties.Pasargad;
using Core.Abstractions.Caching;

namespace Application.Services.Abstractions.Caching
{
    public interface IPasargadBankAccountDetailCacheService : IBaseHCacheService<PasargadBankAccountDetailRs>
    {
        Task<IEnumerable<PasargadBankAccountDetailRs>> GetAll(CancellationToken cancellationToken = default);
        Task<PasargadBankAccountDetailRs> GetByBankAccountId(int bankAccountId, CancellationToken cancellationToken = default);

    }
}
