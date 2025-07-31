using Microsoft.Extensions.DependencyInjection;
using System;
using Presentation.HttpClients.ThirdParties;
using Presentation.HttpClients.ThirdParties.Banks;
using System.Net.Http;
using Application.Services.Abstractions.HttpClients.ThirdParties;
using Application.Services.Abstractions.HttpClients.ThirdParties.BankingProviders;
using Application.Models;
using WebCore.Services.HttpClients.DelegatingHandlers;
using Presentation.HttpClients.ThirdParties.DelegateHandlers;
using Polly;
using Polly.Retry;
using System.Net;
using Core;
using Microsoft.Extensions.Logging;
using Core.Extensions;

namespace Presentation.Configuration
{
    public static class HttpClientRegistration
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, ApplicationSettingExtenderModel applicationSetting)
        {
            services.AddTransient<LoggingDelegateHandler>();
            
            services.AddHttpClient<IPasargadBankClientService, PasargadBankClientService>(client => //PasargadBankClientFakeService
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("_token_", $"{applicationSetting.Banks.Pasargad.Token}");
                client.DefaultRequestHeaders.Add("_token_issuer_", $"{applicationSetting.Banks.Pasargad.TokenIssuer}");

                if (applicationSetting.Banks.MockServer.IsEnabled)
                    client.BaseAddress = new Uri(applicationSetting.Banks.MockServer.ServerUrl);
                else
                    client.BaseAddress = new Uri(applicationSetting.Banks.Pasargad.BaseUrl);

            })
                .AddHttpMessageHandler<PasargadBankDelegateHandler>()
                .AddHttpMessageHandler<LoggingDelegateHandler>()
                .AddResilienceHandler("default", GetResiliencePipeline);

            services.AddHttpClient<IRayanClientService, RayanClientService>(client =>
            {
                client.BaseAddress = new Uri(applicationSetting.Rayan.BaseUrl);
            })
                .AddHttpMessageHandler<LoggingDelegateHandler>();

            //.AddPolicyHandler(GetRetryPolicy())
            //.AddPolicyHandler(GetCircuitBreakerPolicy())

            services.AddTransient<PasargadBankDelegateHandler>();
            services.AddTransient<LoggingDelegateHandler>();
            return services;
        }

        private static void GetResiliencePipeline(ResiliencePipelineBuilder<HttpResponseMessage> builder) =>
             new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions()
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Constant,
                    ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>((e) => (int)e.StatusCode is 227 or 378 or 999 or (int)HttpStatusCode.RequestTimeout),
                    //ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>((e) => (int)e.StatusCode is (int)HttpStatusCode.TooManyRequests),
                    OnRetry = (options) =>
                    {
                        var logger = ServiceLocator.GetService<ILogger<ResiliencePipelineBuilder>>();
                        Console.WriteLine($"Retry #{0}: {1}", options.AttemptNumber, options.Outcome.Result?.ToJsonString());
                        logger.LogWarning(options.Outcome.Exception, $"Retry #{0}: {1}", options.AttemptNumber, options.Outcome.Result?.ToJsonString());

                        return default;
                    }
                })
               .AddTimeout(TimeSpan.FromSeconds(3))
               .Build();

    }
}
