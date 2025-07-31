using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses;
using Core.Abstractions.Caching;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.Abstractions.Caching
{
    public interface ILimitationCacheService : IBaseHCacheService<AllLimitationsRs>
    {
        Task<LimitationRs> GetLimitationByLimitationType(int fundId, LimitationTypeEnum limitationType);
        Task<LimitationRs> GetById(int fundId, int id, CancellationToken cancellationToken = default);

    }
}
