using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses;
using Core.Abstractions.Caching;
using Domain.Entities;

namespace Application.Services.Abstractions.Caching
{
    public interface IUserFundCacheService : IBaseSCacheService<IEnumerable<UserFundRs>>
    {
        Task<IEnumerable<UserFundRs>> GetByUserId(long userId, CancellationToken cancellationToken = default);
        Task<UserFundRs> GetById(long userId, int id, CancellationToken cancellationToken = default);

    }
}
