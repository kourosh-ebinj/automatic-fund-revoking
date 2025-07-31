using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Services.Abstractions;
using Application.Services.Abstractions.ThirdParties.Banks;
using Core;
using Core.Abstractions;
using Core.Extensions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.EntityFramework.ThirdParties.Banks
{
    public class BankingProviderSuggestService : IBankingProviderSuggestService
    {
        protected readonly ILogger _logger;
        protected readonly IGuard _guard;
        protected readonly IReflectionService _reflectionService;
        protected readonly IBankService _bankService;
        protected readonly ApplicationSettingExtenderModel _applicationSetting;
        private readonly IBankAccountSelectorService _bankAccountSelectorService;
        private readonly IPayMethodSelectorService _payMethodSelectorService;

        public BankingProviderSuggestService(IBankService bankService,
                                             IBankAccountSelectorService bankAccountSelectorService,
                                             IPayMethodSelectorService payMethodSelectorService,
                                             ApplicationSettingExtenderModel applicationSetting,
                                             ILogger<BankingProviderFactoryService> logger)
        {
            _bankService = bankService;
            _logger = logger;
            _applicationSetting = applicationSetting;
            _reflectionService = ServiceLocator.GetService<IReflectionService>();
            _guard = ServiceLocator.GetService<IGuard>();
            _bankAccountSelectorService = bankAccountSelectorService;
            _payMethodSelectorService = payMethodSelectorService;
        }

        public async Task<(BankAccount SourceBankAccount, BankPaymentMethodEnum BankPaymentMethod)> GetBestSuggestion(int fundId, long orderId, double orderTotalAmount, int customerAccountBankId, CancellationToken cancellationToken)
        {
            var sourceBankAccount = await _bankAccountSelectorService.GetAppropriateBankAccount(fundId, orderId, customerAccountBankId);
            var paymentMethod = await _payMethodSelectorService.GetAppropriatePaymentMethod(sourceBankAccount.BankId, customerAccountBankId, orderTotalAmount, cancellationToken);

            var result = (sourceBankAccount, paymentMethod);

            var logData = new
            {
                orderId,
                customerAccountBankId,
                result,
            };
            _logger.LogInformation($"{nameof(GetBestSuggestion)}: {0}", logData);

            return result;
        }
    }
}
