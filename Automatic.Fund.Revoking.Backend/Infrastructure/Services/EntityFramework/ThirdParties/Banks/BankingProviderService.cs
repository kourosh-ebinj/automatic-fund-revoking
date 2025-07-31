using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Requests;
using Application.Models.Responses;
using Application.Services.Abstractions;
using Application.Services.Abstractions.ThirdParties.Banks;
using Core;
using Core.Abstractions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.EntityFramework.ThirdParties.Banks
{
    public abstract class BankingProviderService : IBankingProviderService
    {
        protected readonly ILogger _logger;
        protected readonly IGuard _guard;
        protected readonly IReflectionService _reflectionService;
        protected readonly IBankService _bankService;
        protected readonly ApplicationSettingExtenderModel _applicationSetting;

        public BankingProviderService()
        {
            _logger = ServiceLocator.GetService<ILogger<BankingProviderService>>();
            _reflectionService = ServiceLocator.GetService<IReflectionService>();
            _bankService = ServiceLocator.GetService<IBankService>();
            _guard = ServiceLocator.GetService<IGuard>();
            _applicationSetting = ServiceLocator.GetService<ApplicationSettingExtenderModel>(); ;
        }

        public virtual Task<double> GetAccountBalance(BankAccountRs bankAccount, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public virtual Task<BankPaymentMethodResultRs> InternalPayment(BankInternalPaymentRq request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public virtual Task<BankPaymentMethodResultRs> PayaPayment(BankPayaPaymentRq request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public virtual Task<BankPaymentMethodResultRs> SatnaPayment(BankSatnaPaymentRq request, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    }
}
