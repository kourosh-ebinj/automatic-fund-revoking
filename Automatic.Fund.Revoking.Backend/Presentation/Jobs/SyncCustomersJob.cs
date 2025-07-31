using Hangfire;
using System.Threading.Tasks;
using System;
using Application.Jobs.Abstractions;
using Microsoft.Extensions.Logging;
using Application.Services.Abstractions;
using System.Threading;
using Application.Services.Abstractions.ThirdParties;

namespace Presentation.Jobs
{
    public class SyncCustomersJob : ISyncCustomersJob
    {
        private readonly ILogger<SyncCustomersJob> _logger;
        private readonly IRayanService _rayanService;
        private readonly ICustomerService _customerService;
        private readonly IFundService _fundService;

        public SyncCustomersJob(ILogger<SyncCustomersJob> logger,
                                IRayanService rayanService,
                                ICustomerService customerService,
                                IFundService fundService)
        {
            _logger = logger;
            _rayanService = rayanService;
            _customerService = customerService;
            _fundService = fundService;
        }

        [AutomaticRetry(Attempts = 10, DelaysInSeconds = new int[] { 120 })]
        [LatencyTimeout(1800)]
        public async Task Run()
        {
            try
            {
                var jobStartTime = DateTime.Now;
                var jobEndTime = DateTime.MinValue;
                _logger.LogInformation($"Job {nameof(SyncCustomersJob)} started on {jobStartTime.ToShortTimeString()}");

                var cancellationToken = new CancellationToken();

                var funds = await _fundService.GetActiveFunds(cancellationToken);

                foreach (var fund in funds)
                    await _customerService.SyncAllCustomers(fund, cancellationToken);

                jobEndTime = DateTime.Now;
                _logger.LogInformation($"Job {nameof(SyncCustomersJob)} ended on {jobEndTime.ToShortTimeString()}");

                _logger.LogInformation($"Job {nameof(SyncCustomersJob)} lasted {(jobEndTime - jobStartTime).Seconds} seconds");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured in {nameof(SyncCustomersJob)} job: {ex.Message}");

                //throw;
            }
        }
    }
}
