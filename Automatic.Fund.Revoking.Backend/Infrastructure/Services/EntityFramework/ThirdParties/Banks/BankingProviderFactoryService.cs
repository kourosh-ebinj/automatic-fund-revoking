using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Services.Abstractions;
using Application.Services.Abstractions.ThirdParties.Banks;
using Core;
using Core.Abstractions;
using Core.Extensions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.EntityFramework.ThirdParties.Banks
{
    public class BankingProviderFactoryService : IBankingProviderFactoryService
    {
        protected readonly ILogger _logger;
        protected readonly IGuard _guard;
        protected readonly IReflectionService _reflectionService;
        protected readonly IBankService _bankService;
        protected readonly ApplicationSettingExtenderModel _applicationSetting;

        public BankingProviderFactoryService(IBankService bankService,
                                             ApplicationSettingExtenderModel applicationSetting,
                                             ILogger<BankingProviderFactoryService> logger)
        {
            _bankService = bankService;
            _logger = logger;
            _applicationSetting = applicationSetting;
            _reflectionService = ServiceLocator.GetService<IReflectionService>();
            _guard = ServiceLocator.GetService<IGuard>();
        }

        public async Task<IBankingProviderService> InstantiateBankingProvider(int fundId, int bankId, CancellationToken cancellationToken = default)
        {
            _guard.Assert(bankId > 0, Core.Enums.ExceptionCodeEnum.BadRequest, "BankId is invalid");

            var bank = await _bankService.GetById(fundId, bankId, cancellationToken);
            _guard.Assert(bank is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $"No active bank found for this bankId (fundId: {fundId}, bankId: {bankId}).");

            var bankPaymentService = await GetBankingProviderService(fundId, bank.Id, cancellationToken);
            _logger.LogInformation($"{nameof(InstantiateBankingProvider)} result: {bankPaymentService}");

            return bankPaymentService;
        }

        private async Task<IBankingProviderService> GetBankingProviderService(int fundId, int bankId, CancellationToken cancellationToken)
        {
            var bank = await _bankService.GetById(fundId, bankId, cancellationToken);
            _guard.Assert(bank.IsEnabled, Core.Enums.ExceptionCodeEnum.NotFound, "بانک موزد نظر یافت نشد و یا غیرفعال است.");

            var instance = ServiceLocator.GetService<IPasargadBankingProviderService>();

            return instance;
        }
    }
}
