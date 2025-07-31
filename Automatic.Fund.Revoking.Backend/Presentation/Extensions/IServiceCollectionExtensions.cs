using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Presentation.Scheduler;
using Hangfire.SqlServer;
using System.Reflection;
using MassTransit;
using Application.Models;
using Infrastructure.Services.Messaging.Consumers;

namespace Presentation.Extensions
{
    public static class IServiceCollectionExtensions
    {

        public static IServiceCollection AddMassTransit(this IServiceCollection services, ApplicationSettingExtenderModel applicationSetting, params Assembly[] consumerAssemblies)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(consumerAssemblies);
                x.UsingRabbitMq((context, cfg) =>
                {
                    // We need  to install rabbitmq_delayed_message_exchange plugin
                    //cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5)));

                    cfg.UseMessageRetry(r => r.Incremental(3, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 3)));

                    cfg.Host(applicationSetting.MassTransit.Host, applicationSetting.MassTransit.Port, applicationSetting.MassTransit.VirtualHost, h =>
                    {
                        h.Username(applicationSetting.MassTransit.Username);
                        h.Password(applicationSetting.MassTransit.Password);

                        //h.UseSsl(s =>
                        //{
                        //    s.Protocol = SslProtocols.Tls12;
                        //});
                        cfg.ReceiveEndpoint(Application.Constants.Queue_MarkOrderAsConfirmed, (endpointCfg) =>
                        {
                            endpointCfg.ConfigureConsumer<MarkOrderAsConfirmedConsumer>(context);

                            //endpointCfg.DiscardFaultedMessages();
                            //endpointCfg.UseMessageRetry(r => r.Immediate(5));
                            endpointCfg.UseRateLimit(5, new TimeSpan(0, 0, 0, 1));
                            //endpointCfg.ExchangeType = "topic";
                            endpointCfg.PrefetchCount = 1;
                            //endpointCfg.BindDeadLetterQueue("dead-letter");

                            //endpointCfg.UseRetry<(retryCfg => { });
                            //endpointCfg.ConfigureDeadLetter(context =>
                            //{

                            //    context.UseBind(recieveContext =>
                            //    {

                            //    });

                            //});
                        });
                        cfg.ReceiveEndpoint(Application.Constants.Queue_ReverseOrderToDraft, (endpointCfg) =>
                        {
                            endpointCfg.ConfigureConsumer<ReverseOrderToDraftConsumer>(context);

                            //endpointCfg.DiscardFaultedMessages();
                            //endpointCfg.UseMessageRetry(r => r.Immediate(5));
                            endpointCfg.UseRateLimit(5, new TimeSpan(0, 0, 0, 1));
                            //endpointCfg.ExchangeType = "topic";
                            endpointCfg.PrefetchCount = 1;
                            //endpointCfg.BindDeadLetterQueue("dead-letter");

                            //endpointCfg.UseRetry<(retryCfg => { });
                            //endpointCfg.ConfigureDeadLetter(context =>
                            //{

                            //    context.UseBind(recieveContext =>
                            //    {

                            //    });

                            //});
                        });
                        cfg.ReceiveEndpoint(Application.Constants.Queue_PayOrder, (endpointCfg) =>
                        {
                            endpointCfg.ConfigureConsumer<PayOrderConsumer>(context);

                            //endpointCfg.DiscardFaultedMessages();
                            //endpointCfg.UseMessageRetry(r => r.Immediate(5));
                            endpointCfg.UseRateLimit(5, new TimeSpan(0, 0, 0, 1));
                            //endpointCfg.ExchangeType = "topic";
                            endpointCfg.PrefetchCount = 1;
                            //endpointCfg.BindDeadLetterQueue("dead-letter");

                            //endpointCfg.UseRetry<(retryCfg => { });
                            //endpointCfg.ConfigureDeadLetter(context =>
                            //{

                            //    context.UseBind(recieveContext =>
                            //    {

                            //    });

                            //});
                        });
                        //cfg.ReceiveEndpoint("message-queue_Fault", (endpointCfg) =>
                        //{
                        //    endpointCfg.Consumer<MessageFaultConsumer>();

                        //});
                        //cfg.ReceiveEndpoint("fault", (endpointCfg) =>
                        //{
                        //    endpointCfg.Consumer<FaultConsumer>();

                        //});
                    });
                    //cfg.Host("localhost", "/", h =>
                    //{
                    //    h.Username("guest");
                    //    h.Password("guest");
                    //});
                });
            });
            return services;
        }

        public static void ConfigHangfire(this IServiceCollection services, IConfiguration configuration, string appName, IWebHostEnvironment webHostEnvironment)
        {
            var connectionString = configuration.GetConnectionString("HangFireConnection");
            var sqlServerOptions = new SqlServerStorageOptions
            {
                PrepareSchemaIfNecessary = true,
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(15),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(15),
                CommandTimeout = TimeSpan.FromMinutes(15),
                TransactionTimeout = TimeSpan.FromMinutes(15),
            };
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, sqlServerOptions);

            //var connectionString = configuration.GetDecryptedConnectionString("HangFireConnection", webHostEnvironment);
            services.AddHangfire(o => o.UseSqlServerStorage(connectionString));

            services.AddHangfireServer();
            //services.AddHangfireServer(x =>
            //{
            //    x.Queues = new[] { appName.ToLower() };
            //    //x.StopTimeout = TimeSpan.FromSeconds(10);
            //    x.SchedulePollingInterval = TimeSpan.FromSeconds(10);
            //});
            GlobalJobFilters.Filters.Add(new PreserveOriginalQueueAttribute());
            GlobalJobFilters.Filters.Add(new DeleteConcurrentExecutionAttribute());
            GlobalJobFilters.Filters.Add(new DisableConcurrentExecutionAttribute(15));
        }
    }
}
