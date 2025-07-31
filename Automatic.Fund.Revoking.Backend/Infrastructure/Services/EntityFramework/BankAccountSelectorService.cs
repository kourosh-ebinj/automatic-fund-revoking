using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Services.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.EntityFramework
{

    public class BankAccountSelectorService : IBankAccountSelectorService
    {
        private readonly ILogger _logger;
        private readonly IBankAccountService _bankAccountService;

        public BankAccountSelectorService(ILogger<BankAccountSelectorService> logger, IBankAccountService bankAccountService)
        {
            _logger = logger;
            _bankAccountService = bankAccountService;

        }

        public async Task<BankAccount> GetAppropriateBankAccount(int fundId, long orderId, int customerAccountBankId, CancellationToken cancellationToken = default)
        {
            var sourceBankAccount = await _bankAccountService.GetAllQueryable(fundId, true).
                                            OrderByDescending(e => e.Balance).
                                            FirstOrDefaultAsync(e => e.BankId == customerAccountBankId, cancellationToken);

            var alternativeAccount = await _bankAccountService.GetAllQueryable(fundId, true).
                                            OrderByDescending(e => e.Balance).
                                            FirstOrDefaultAsync(cancellationToken);

            var logContext = new
            {
                orderId,
                sourceBankAccount,
                alternativeAccount
            };
            _logger.LogInformation($"AppropriateBankAccount for orderId: {orderId}: {0}", logContext);

            if (sourceBankAccount is not null) // We already support customer's bank
                return sourceBankAccount;

            // We already do not support customer's bank
            return alternativeAccount;
        }
    }
}
