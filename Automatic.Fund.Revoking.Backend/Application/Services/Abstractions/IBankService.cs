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
    public interface IBankService : ICRUDService<Bank>
    {
        Task<IEnumerable<BankRs>> GetAll(int fundId, bool? isEnabled = null);
        IQueryable<Bank> GetAllQueryable(int fundId, bool? isEnabled = null);
        Task<BankRs> GetById(int fundId, int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<BankRs>> GetActiveBanks(int fundId, CancellationToken cancellationToken = default);
        Task<BankRs> GetActiveBankById(int fundId, int bankId, CancellationToken cancellationToken = default);
        Task<BankRs> Update(int fundId, BankUpdateRq request, CancellationToken cancellationToken = default);

    }
}
