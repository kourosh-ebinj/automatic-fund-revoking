using Hangfire;
using System.Threading.Tasks;
using System;
using Application.Jobs.Abstractions;
using Microsoft.Extensions.Logging;
using Application.Services.Abstractions;
using Application.Models;
using System.Threading;
using Core.Services;
using Application;
using Application.Services.Abstractions.ThirdParties.Banks;
using System.Linq;
using Core.Extensions;
using Core;

namespace Presentation.Jobs
{
    public class GetAccountBalanceJob : IGetAccountBalanceJob
    {
        private readonly ILogger<GetAccountBalanceJob> _logger;
        private readonly IBankingProviderFactoryService _bankingProviderFactoryService;
        private readonly IFundService _fundService;
        private readonly IBankAccountService _bankAccountService;
        private readonly ApplicationSettingExtenderModel _applicationSettings;

        public GetAccountBalanceJob(ILogger<GetAccountBalanceJob> logger,
                                    IFundService fundService,
                                    IBankAccountService bankAccountService,
                                    IBankingProviderFactoryService bankingProviderFactoryService,
                                    ILimitationComponentService limitationComponentService,
                                    ApplicationSettingExtenderModel applicationSettings)
        {
            _logger = logger;
            _fundService = fundService;
            _bankAccountService = bankAccountService;
            _bankingProviderFactoryService = bankingProviderFactoryService;
            _applicationSettings = applicationSettings;

        }

        [AutomaticRetry(Attempts = 10, DelaysInSeconds = new int[] { 120 })]
        public async Task Run()
        {
            Console.WriteLine($"Job {nameof(GetAccountBalanceJob)} executed on {DateTime.Now.ToShortTimeString()}");
            try
            {
                var cancellationToken = new CancellationToken();
                var funds = await _fundService.GetActiveFunds(cancellationToken);

                foreach (var fund in funds)
                {
                    var bankAccounts = await _bankAccountService.GetBankAccountsByFundId(fund.Id, true, cancellationToken);
                    foreach (var bankAccount in bankAccounts.Where(e => e.IsEnabled))
                    {
                        var bankProviderInstance = await _bankingProviderFactoryService.InstantiateBankingProvider(fund.Id, bankAccount.BankId, cancellationToken);
                        var balance = await bankProviderInstance.GetAccountBalance(bankAccount, cancellationToken);

                        await _bankAccountService.UpdateBalance(fund.Id, bankAccount.Id, balance);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
