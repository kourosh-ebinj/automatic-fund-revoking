using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Core;
using Microsoft.AspNetCore.Http;
using WebCore.Extensions;
using Core.Constants;
using Microsoft.Extensions.Configuration;
using Application.Models;

namespace Presentation.HttpClients.ThirdParties.DelegateHandlers
{
    public class PasargadBankDelegateHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationSettingExtenderModel _configuration;

        public PasargadBankDelegateHandler(ILogger<PasargadBankDelegateHandler> logger,
                                           IHttpContextAccessor httpContextAccessor,
                                           ApplicationSettingExtenderModel configuration)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                if (_configuration.Banks.MockServer.IsEnabled)
                {
                    request.Headers.Add("x-mock-response-code", "200");
                    request.Headers.Add("x-mock-match-request-headers", "true");
                    //_httpClient.DefaultRequestHeaders.Add("x-mock-match-request-body", "true");
                }

                var response = await base.SendAsync(request, cancellationToken);

                return response;
            }
            catch (HttpRequestException ex)
            {
                var logData = new
                {
                    Request = request,
                    CorrelationId = _httpContextAccessor.HttpContext.GetCorrelationId(),
                    ex.HttpRequestError
                };

                if ((int)ex.StatusCode == 21) // Invalid Token
                {
                    _logger.LogCritical(ex, ex.Message, logData);
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                else if ((int)ex.StatusCode == 227) // Timed out error
                {
                    _logger.LogCritical(ex, ex.Message, logData);
                    //return new HttpResponseMessage(HttpStatusCode.RequestTimeout);
                    
                    // We throw the exception to be catched by retry mechanism
                    throw;

                }
                else if ((int)ex.StatusCode == 378) // Unhandled error
                {
                    _logger.LogCritical(ex, ex.Message, logData);
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }

                throw;
            }
            catch (Exception ex)
            {
                var logData = new
                {
                    Request = request,
                    CorrelationId = _httpContextAccessor.HttpContext.GetCorrelationId(),
                };
                _logger.LogError(ex, "HttpClient call encountered with an error: {0}", logData);
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }
    }

}
