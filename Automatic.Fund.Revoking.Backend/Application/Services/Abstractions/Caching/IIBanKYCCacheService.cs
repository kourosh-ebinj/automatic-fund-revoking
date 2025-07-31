using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Abstractions.Caching;
using Domain.Entities;

namespace Application.Services.Abstractions.Caching
{
    public interface IIBanKYCCacheService : IBaseHCacheService<IBanKYC>
    {
        Task<IEnumerable<IBanKYC>> GetAll(CancellationToken cancellationToken = default);
        Task<IBanKYC> GetByIBan(string iban, CancellationToken cancellationToken = default);

    }
}
