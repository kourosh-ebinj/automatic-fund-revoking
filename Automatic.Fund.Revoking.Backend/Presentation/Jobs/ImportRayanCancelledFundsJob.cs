using Hangfire;
using System.Threading.Tasks;
using System;
using Application.Jobs.Abstractions;
using Microsoft.Extensions.Logging;
using Application.Services.Abstractions;
using System.Threading;

namespace Presentation.Jobs
{
    public class ImportRayanCancelledFundsJob : IImportRayanCancelledFundsJob
    {
        private readonly ILogger<ImportRayanCancelledFundsJob> _logger;
        private readonly IOrderProcessingService _orderProcessor;
        private readonly IFundService _fundService;

        public ImportRayanCancelledFundsJob(ILogger<ImportRayanCancelledFundsJob> logger,
                                            IFundService fundService,
                                            IOrderProcessingService orderProcessor)
        {
            _logger = logger;
            _orderProcessor = orderProcessor;
            _fundService = fundService;
        }

        [AutomaticRetry(Attempts = 10, DelaysInSeconds = new int[] { 120 })]
        public async Task Run()
        {
            try
            {
                Console.WriteLine($"Job {nameof(ImportRayanCancelledFundsJob)} executed on {DateTime.Now.ToShortTimeString()}");

                var cancellationToken = new CancellationToken();
                var funds = await _fundService.GetActiveFunds(cancellationToken);

                foreach (var fund in funds)
                {
                    var startDate = DateTime.Now;
                    var endDate = DateTime.Today.AddDays(1);

                    await _orderProcessor.ImportRayanCancelledFundsToOrders(fund.Id, startDate, endDate);
                }
            }
            catch (InvalidOperationException ex) when (ex.Message.StartsWith("A second operation was started on this context instance before a previous operation completed"))
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
