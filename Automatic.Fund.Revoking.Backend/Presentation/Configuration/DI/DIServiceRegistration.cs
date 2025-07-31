using System;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Services.EntityFramework;
using Core.Services;
using Application.Services;
using Application.Services.Abstractions;
using Application.Services.Abstractions.ThirdParties;
using Infrastructure.Services;
using Application.Jobs.Abstractions;
using Presentation.Jobs;
using Application.Services.Abstractions.ThirdParties.Banks;
using Core.Extensions;
using Infrastructure.Persistence.Providers.EntityFramework;
using Application.Services.Abstractions.Audit;
using Application.Services.Abstractions.Persistence;
using Infrastructure.Services.EntityFramework.Audit;
using Core.Abstractions.Caching;
using Application.Services.Abstractions.Caching;
using Infrastructure.Services.Caching;
using Infrastructure.Services.EntityFramework.ThirdParties;
using Infrastructure.Services.EntityFramework.ThirdParties.Banks;
using WebCore.Services.Caching;

namespace Presentation.Configuration.DI
{
    public static class DIServiceRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection services, ApplicationSettingModel applicationSetting)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ILimitationComponentValidatorService, LimitationComponentValidatorService>();
            services.AddScoped<ILimitationComponentService, LimitationComponentService>();
            services.AddScoped<ILimitationService, LimitationService>();
            services.AddScoped<IBankAccountService , BankAccountService>();
            services.AddScoped<IBankAccountSelectorService, BankAccountSelectorService>();
            services.AddScoped<IPayMethodSelectorService, PayMethodSelectorService>();
            services.AddScoped<IBankingProviderFactoryService, BankingProviderFactoryService>();
            services.AddScoped<IBankingProviderSuggestService, BankingProviderSuggestService>();
            services.AddScoped<IBankPaymentService, BankPaymentService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IRayanService, RayanService>();
            services.AddScoped<ISagaTransactionService, SagaTransactionService>();
            services.AddScoped<IOrderProcessingService, OrderProcessingService>();
            services.AddScoped<IOrderHistoryService, OrderHistoryService>();
            services.AddScoped<IIBanKYCService, IBanKYCService>();
            services.AddScoped<IFundService, FundService>();
            services.AddScoped<IUserFundService, UserFundService>();
            services.AddScoped<IRayanFundOrderService, RayanFundOrderService>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IPasargadBankingProviderService, PasargadBankingProviderService>();
            services.AddScoped<IBankingProviderFactoryService, BankingProviderFactoryService>();
            services.AddScoped<IBankingProviderFactoryService, BankingProviderFactoryService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IRayanCustomerService, RayanCustomerService>();
            services.AddScoped<IRayanFundOrderService, RayanFundOrderService>();
            services.AddScoped<IPasargadBankAccountDetailService, PasargadBankAccountDetailService>();
            
            // Jobs
            services.AddScoped<IGetAccountBalanceJob, GetAccountBalanceJob>();
            services.AddScoped<IImportRayanCancelledFundsJob, ImportRayanCancelledFundsJob>();
            services.AddScoped<IPayJob, PayJob>();
            services.AddScoped<ISyncCustomersJob, SyncCustomersJob>();

            // Cache Services
            services.AddScoped<IIBanKYCCacheService, IBanKYCCacheService>();
            services.AddScoped<IFundCacheService, FundCacheService>();
            services.AddScoped<IUserFundCacheService, UserFundCacheService>();
            services.AddScoped<IBankCacheService, BankCacheService>();
            services.AddScoped<IBankAccountCacheService, BankAccountCacheService>();
            services.AddScoped<ILimitationCacheService, LimitationCacheService>();
            services.AddScoped<ILimitationCacheService, LimitationCacheService>();
            services.AddScoped<ILimitationComponentCacheService, LimitationComponentCacheService>();
            services.AddScoped<IPasargadBankAccountDetailCacheService, PasargadBankAccountDetailCacheService>();

            services.AddSingleton<ISMSService, SMSService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<ICachingService, InMemoryCacheService>();
            services.AddSingleton<ICacheProvider, RedisCacheProvider>();

            services.AddBaseServices(applicationSetting);
            return services;
        }

    }
}
