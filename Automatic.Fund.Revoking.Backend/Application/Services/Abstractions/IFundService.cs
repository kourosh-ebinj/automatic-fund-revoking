using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Entities;

namespace Application.Services.Abstractions
{
    public interface IFundService : ICRUDService<Fund>
    {
        Task<FundRs> GetById(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<FundRs>> GetAll(long userId, IEnumerable<string> roles, bool? isEnabled = null);
        IQueryable<Fund> GetAllQueryable(bool? isEnabled = null);
        Task<IEnumerable<FundRs>> GetActiveFunds(CancellationToken cancellationToken = default);
        Task<FundRs> GetActiveFundById(int fundId, CancellationToken cancellationToken = default);

        Task<FundRs> Create(FundCreateRq request, CancellationToken cancellationToken = default);
        Task<FundRs> Update(FundUpdateRq request, CancellationToken cancellationToken = default);
        Task Delete(int id, CancellationToken cancellationToken = default);

    }
}
