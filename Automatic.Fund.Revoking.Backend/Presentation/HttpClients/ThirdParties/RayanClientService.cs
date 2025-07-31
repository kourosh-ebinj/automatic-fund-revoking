using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums;
using Application.Models;
using Application.Models.Requests.ThirdParties.Rayan;
using Application.Models.Responses.ThirdParties.Rayan;
using Application.Services.Abstractions.HttpClients.ThirdParties;
using AutoMapper;
using Core.Extensions;
using Core.Helpers;
using Core.Services;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using WebCore.Services.HttpClients;

namespace Presentation.HttpClients.ThirdParties
{
    public class RayanClientService : HttpClientService<RayanClientService>, IRayanClientService
    {

        private readonly ICachingService _cacheService;
        private readonly ApplicationSettingExtenderModel _configuration;

        IMapper _mapper;
        public RayanClientService(HttpClient httpClient,
                                  ICachingService cacheService,
                                  ApplicationSettingExtenderModel configuration,
                                  IMapper mapper) : base(httpClient)
        {
            _mapper = mapper;
            _cacheService = cacheService;
            _configuration = configuration;
        }

        public async Task<IEnumerable<RayanFundOrderRs>> GetFundRequests(int dsCode, DateTime startDate, DateTime endDate,
                                                                           int pageNumber = 1, int pageSize = IRayanClientService.PAGESIZE,
                                                                           CancellationToken cancellationToken = default)
        {
            var loginInfo = await Authenticate(cancellationToken);

            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"dsCode={dsCode}");
            queryBuilder.Append($"&startDate={startDate.ToDateTimeString("yyyy/MM/dd", convertToShamsi: true)}");
            queryBuilder.Append($"&endDate={endDate.ToDateTimeString("yyyy/MM/dd", convertToShamsi: true)}");
            queryBuilder.Append($"&size={pageSize}");
            queryBuilder.Append($"&page={pageNumber}");
            queryBuilder.Append($"&orderBy=fundOrderId_asc");
            //queryBuilder.Append($"&offset=0"); // number of items to skip in the current page

            var uri = new Uri(_httpClient.BaseAddress, "/api/v1/fundRevokeOrders" + $"?{queryBuilder}");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-CLIENT-TOKEN", loginInfo.ClientToken);

            try
            {
                var result = await _httpClient.GetFromJsonAsync<RayanListBase<RayanFundOrderRs>>(uri, cancellationToken: cancellationToken);
                return result.Result;
            }
            catch (HttpRequestException ex)
            {
                var logData = new { dsCode, startDate, endDate };
                ex.Data.Add("Method", logData);
                _logger.LogError(ex, "GetFundRequests failed. because: {0}", logData);

                return new List<RayanFundOrderRs>();
            }
        }

        public async Task<RayanListBase<RayanCustomerRs>> GetCustomersList(int dsCode, int pageNumber = 1, int pageSize = IRayanClientService.PAGESIZE,
                                                                           CancellationToken cancellationToken = default)
        {
            var loginInfo = await Authenticate(cancellationToken);

            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"dsCode={dsCode}");
            queryBuilder.Append($"&size={pageSize}");
            queryBuilder.Append($"&page={pageNumber}");
            queryBuilder.Append($"&orderBy=customerId");
            //queryBuilder.Append($"&offset=0"); // number of items to skip in the current page

            var uri = new Uri(_httpClient.BaseAddress, "/api/v1/listCustomers" + $"?{queryBuilder}");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-CLIENT-TOKEN", loginInfo.ClientToken);

            try
            {
                var result = await _httpClient.GetFromJsonAsync<RayanListBase<RayanCustomerRs>>(uri, cancellationToken: cancellationToken);
                return result;
            }
            catch (HttpRequestException ex)
            {
                //var logData = new { dsCode};
                //ex.Data.Add("Method", logData);
                //_logger.LogError(ex, "GetFundRequests failed. because: {0}", logData); 
                throw;
            }
        }

        public async Task<RayanCustomerInfoRs> GetCustomerInfo(int dsCode, string nationalCode,
                                                               CancellationToken cancellationToken = default)
        {
            try
            {

                var loginInfo = await Authenticate(cancellationToken);

                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"dsCode={dsCode}");
                queryBuilder.Append($"&nationalCode={nationalCode}");

                var uri = new Uri(_httpClient.BaseAddress, "/api/v1/customers/customerInfo" + $"?{queryBuilder}");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-CLIENT-TOKEN", loginInfo.ClientToken);

                var result = await _httpClient.GetFromJsonAsync<RayanCustomerInfoRs>(uri, cancellationToken: cancellationToken);

                return result;

            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"NationalCode ({nationalCode}) not found in dsCode ({dsCode})");
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateOrderStatus(int dsCode, long orderId, string orderDate, RayanStatusEnum fromStatus, RayanStatusEnum toStatus,
                                                  CancellationToken cancellationToken = default)
        {
            try
            {
                var loginInfo = await Authenticate(cancellationToken);
                var boLoginInfo = await AuthenticateBackOffice(dsCode, cancellationToken);

                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"dsCode={dsCode}");

                var uri = new Uri(_httpClient.BaseAddress, "/api/v1/bo/fundOrder/changeStatus/change" + $"?{queryBuilder}");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-CLIENT-TOKEN", loginInfo.ClientToken);
                _httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + boLoginInfo.AuthCode);

                var body = new
                {
                    fundId = 1.ToString(),
                    date = orderDate,
                    fromOrderStatus = fromStatus.ToString(),
                    toOrderStatus = toStatus.ToString(),
                    orderType = "REVOKE",
                    fundOrderIds = new List<string>() { orderId.ToString() },
                };

                var result = await _httpClient.PostAsync(uri, body.ToHttpContent(), cancellationToken: cancellationToken);

                if (result.IsSuccessStatusCode)
                    return true;
                else
                {
                    var jToken = JsonHelper.GetItemByPath(await result.Content.ReadAsStringAsync(), "$.error");
                    var errorCode = jToken["errorCode"];
                    var description = jToken["description"];
                    _logger.LogWarning($"Updating orde status failed for OrderId ({orderId}): {description} ({errorCode})");

                    return false;
                }

            }
            catch (HttpRequestException ex)
            {
                var logData = new
                {
                    dsCode,
                    orderId,
                    fromStatus,
                    toStatus,
                };
                _logger.LogError(ex, "An error occured in updating fund state: " + logData.ToJsonString());

                return false;
            }
        }

        private async Task<RayanAuthenticationRs> Authenticate(CancellationToken cancellationToken)
        {
            RayanAuthenticationRs loginInfo = null;
            if (!await _cacheService.TryGet(Application.Constants.CacheKey_RayanAuthenticateToken, out loginInfo))
            {
                var uri = new Uri(_httpClient.BaseAddress, "/api/v1/authenticate");

                var model = new RayanAuthenticationRq()
                {
                    username = _configuration.Rayan.Username,
                    password = _configuration.Rayan.Password,
                };

                var response = await _httpClient.PostAsJsonAsync(uri.ToString(), model, cancellationToken: cancellationToken);

                loginInfo = await response.Content.ReadFromJsonAsync<RayanAuthenticationRs>();
                await _cacheService.SetAsync(Application.Constants.CacheKey_RayanAuthenticateToken, loginInfo, new TimeSpan(12, 0, 0));
            }

            return loginInfo;
        }

        private async Task<RayanBOAuthenticationRs> AuthenticateBackOffice(int dsCode, CancellationToken cancellationToken)
        {
            if (!await _cacheService.TryGet(Application.Constants.CacheKey_RayanBackOfficeAuthenticateToken, out RayanBOAuthenticationRs boLoginInfo))
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"dsCode={dsCode}");

                var loginInfo = await Authenticate(cancellationToken);

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-CLIENT-TOKEN", loginInfo.ClientToken);

                var uri = new Uri(_httpClient.BaseAddress, "/api/v1/bo/login" + $"?{queryBuilder}");

                var model = new RayanAuthenticationRq()
                {
                    username = _configuration.Rayan.BackOffice.Username,
                    password = _configuration.Rayan.BackOffice.Password,
                };

                var response = await _httpClient.PostAsJsonAsync(uri.ToString(), model, cancellationToken: cancellationToken);

                boLoginInfo = await response.Content.ReadFromJsonAsync<RayanBOAuthenticationRs>();
                if (boLoginInfo is not null)
                    await _cacheService.SetAsync(Application.Constants.CacheKey_RayanBackOfficeAuthenticateToken, loginInfo, new TimeSpan(12, 0, 0));
            }

            return boLoginInfo;
        }
    }
}
