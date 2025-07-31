using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses;
using Core.Abstractions.Caching;

namespace Application.Services.Abstractions.Caching
{
    public interface ILimitationComponentCacheService : IBaseHCacheService<LimitationComponentRs>
    {
        Task<IEnumerable<LimitationComponentRs>> GetAll(int limitationId, bool? isEnabled = null, CancellationToken cancellationToken = default);
        Task<LimitationComponentRs> GetById(int limitationId, int id, CancellationToken cancellationToken = default);
    }
}
