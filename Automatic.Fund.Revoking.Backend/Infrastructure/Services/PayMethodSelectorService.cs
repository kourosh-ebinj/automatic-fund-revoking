using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Services.Abstractions;
using Domain.Enums;
using Microsoft.Extensions.Logging;
namespace Infrastructure.Services
{

    public class PayMethodSelectorService : IPayMethodSelectorService
    {
        private readonly ILogger _logger;
        public readonly ApplicationSettingExtenderModel _applicationSetting;

        public PayMethodSelectorService(ILogger<PayMethodSelectorService> logger, ApplicationSettingExtenderModel applicationSetting)
        {
            _logger = logger;
            _applicationSetting = applicationSetting;

        }

        public async ValueTask<BankPaymentMethodEnum> GetAppropriatePaymentMethod(int sourceBankId, int destinationBankId, double amount, CancellationToken cancellationToken)
        {
            BankPaymentMethodEnum suggestedPaymentMethod;

            if (false) //(sourceBankId == destinationBankId)
                suggestedPaymentMethod = BankPaymentMethodEnum.Internal;
            else if (amount <= _applicationSetting.Banks.Markazi.PayaMax)
                suggestedPaymentMethod = BankPaymentMethodEnum.Paya;
            else
                suggestedPaymentMethod = BankPaymentMethodEnum.Satna;

            var logData = new
            {
                sourceBankId,
                destinationBankId,
                amount,
                suggestedPaymentMethod
            };
            _logger.LogInformation($"{nameof(GetAppropriatePaymentMethod)} SuggestedPaymentMethod: {0}", logData);

            return suggestedPaymentMethod;
        }

    }
}
