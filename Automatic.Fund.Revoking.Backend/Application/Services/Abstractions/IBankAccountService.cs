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
    public interface IBankAccountService : ICRUDService<BankAccount>
    {
        Task<BankAccountRs> GetById(int fundId, int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<BankAccountRs>> GetAll(int fundId, bool? isEnabled = null, CancellationToken cancellationToken = default);
        IQueryable<BankAccount> GetAllQueryable(int fundId, bool? isEnabled = null);
        Task<IEnumerable<BankAccountRs>> GetBankAccountsByFundId(int fundId, bool? isEnabled = null, CancellationToken cancellationToken = default);
        Task<BankAccountRs> Create(int fundId, BankAccountCreateRq request, CancellationToken cancellationToken = default);
        Task<BankAccountRs> Update(int fundId, BankAccountUpdateRq request, CancellationToken cancellationToken = default);
        Task<BankAccountRs> UpdateBalance(int fundId, int id, double balance, CancellationToken cancellationToken = default);
        Task Delete(int fundId, int id, CancellationToken cancellationToken = default);

    }
}
