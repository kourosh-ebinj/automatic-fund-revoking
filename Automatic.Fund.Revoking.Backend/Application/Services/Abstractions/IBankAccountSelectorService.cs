using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Services.Abstractions
{
    public interface IBankAccountSelectorService
    {
        Task<BankAccount> GetAppropriateBankAccount(int fundId, long orderId, int customerAccountBankId, CancellationToken cancellationToken = default);

    }
}
