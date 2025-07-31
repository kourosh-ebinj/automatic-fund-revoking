using Hangfire;
using System.Threading.Tasks;
using System;
using Application.Jobs.Abstractions;
using Microsoft.Extensions.Logging;
using Application.Services.Abstractions;
using System.Threading;

namespace Presentation.Jobs
{
    public class PayJob : IPayJob
    {
        private readonly ILogger<PayJob> _logger;
        private readonly IFundService _fundService;
        private readonly IOrderProcessingService _orderProcessingService;

        public PayJob(ILogger<PayJob> logger,
                      IFundService fundService,
                      IOrderProcessingService orderProcessingService)
        {
            _logger = logger;
            _fundService = fundService;
            _orderProcessingService = orderProcessingService;
        }

        [AutomaticRetry(Attempts = 10, DelaysInSeconds = new int[] { 120 })]
        public async Task Run()
        {
            try
            {
                Console.WriteLine($"Job {nameof(PayJob)} executed on {DateTime.Now.ToShortTimeString()}");

                var cancellationToken = new CancellationToken();
                var funds = await _fundService.GetActiveFunds(cancellationToken);

                foreach (var fund in funds)
                    await _orderProcessingService.PayAcceptedOrders(fund.Id, cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
