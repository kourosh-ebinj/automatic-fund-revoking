using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Application.Jobs.Abstractions;
using Core.Constants;
using Application.Models;

namespace Presentation.Jobs
{
    public class JobScheduler
    {
        public static void ScheduleJobs(IApplicationBuilder app, ApplicationSettingExtenderModel applicationSettings)
        {

            var appName = applicationSettings.App.Name;

            RecurringJob.AddOrUpdate<IImportRayanCancelledFundsJob>($"{appName}.{nameof(ImportRayanCancelledFundsJob)}",
                j => j.Run(), Cron.Minutely, new RecurringJobOptions()
                {
                    //QueueName = appName.ToLower(),
                    TimeZone = GlobalConstants.GetTehranTimeZoneInfo(),
                    //MisfireHandling = MisfireHandlingMode.Relaxed;
                });

            RecurringJob.AddOrUpdate<IGetAccountBalanceJob>($"{appName}.{nameof(GetAccountBalanceJob)}",
                j => j.Run(), Cron.HourInterval(3), new RecurringJobOptions()
                {
                    //QueueName = appName.ToLower(),
                    TimeZone = GlobalConstants.GetTehranTimeZoneInfo(),
                    //MisfireHandling = MisfireHandlingMode.Relaxed;
                });

            RecurringJob.AddOrUpdate<IPayJob>($"{appName}.{nameof(PayJob)}",
                j => j.Run(), Cron.HourInterval(3), new RecurringJobOptions()
                {
                    //QueueName = appName.ToLower(),
                    TimeZone = GlobalConstants.GetTehranTimeZoneInfo(),
                    //MisfireHandling = MisfireHandlingMode.Relaxed;
                });

            RecurringJob.AddOrUpdate<ISyncCustomersJob>($"{appName}.{nameof(SyncCustomersJob)}",
                j => j.Run(), Cron.Daily(1, 0), new RecurringJobOptions()
                {
                    //QueueName = appName.ToLower(),
                    TimeZone = GlobalConstants.GetTehranTimeZoneInfo(),
                    //MisfireHandling = MisfireHandlingMode.Relaxed;
                });
        }
    }
}
