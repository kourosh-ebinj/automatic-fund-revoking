using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses;
using Core.Abstractions.Caching;
using Domain.Entities;

namespace Application.Services.Abstractions.Caching
{
    public interface IFundCacheService : IBaseSCacheService<IEnumerable<FundRs>>
    {
        Task<IEnumerable<FundRs>> GetAll(bool? isEnabled = null);
        Task<FundRs> GetById(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<FundRs>> GetActiveFunds(CancellationToken cancellationToken = default);
        Task<FundRs> GetActiveFundById(int fundId, CancellationToken cancellationToken = default);

    }
}
