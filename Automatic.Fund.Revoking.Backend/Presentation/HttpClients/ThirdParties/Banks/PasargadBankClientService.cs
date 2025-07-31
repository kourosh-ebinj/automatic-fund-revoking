using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Requests.ThirdParties.Pasargad;
using Application.Models.Responses.ThirdParties.Pasargad;
using Application.Services.Abstractions.HttpClients.ThirdParties.BankingProviders;
using Application.Services.Abstractions.ThirdParties.Banks;
using Core.Constants;
using Core.Extensions;
using Core.Helpers;
using Infrastructure.Services.EntityFramework.ThirdParties.Banks;
using MassTransit;
using WebCore.Services.HttpClients;

namespace Presentation.HttpClients.ThirdParties.Banks
{
    public class PasargadBankClientService : HttpClientService<PasargadBankClientService>, IPasargadBankClientService
    {
        private readonly ApplicationSettingExtenderModel _configuration;

        public PasargadBankClientService(HttpClient httpClient, ApplicationSettingExtenderModel configuration) : base(httpClient)
        {
            _configuration = configuration;
        }

        public async Task<PasargadBaseRs<PasargadBankInternalPaymentRs>> InternalPayment(PasargadBankInternalPaymentRq request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_httpClient.BaseAddress, "/srv/sc/nzh/doServiceCall");

            if (_configuration.Banks.MockServer.IsEnabled)
                AddPostmanResponseNameToHeader(_httpClient, "InternalPayment");

            var collection = new List<KeyValuePair<string, string>>
            {
                new("scProductId", $"{_configuration.Banks.Pasargad.ProductIdInternal}"),
                new("scApiKey", $"{request.ScApiKeyInternal}"),
                new("request", request.ToJsonString()),
            };
            var content = new FormUrlEncodedContent(collection);

            var response = await _httpClient.PostAsync(uri, content, cancellationToken: cancellationToken);

            if (!response.IsSuccessStatusCode)
                return new PasargadBaseRs<PasargadBankInternalPaymentRs>()
                {
                    HasError = true,
                    ErrorCode = 0,
                    ErrorMessage = response.ReasonPhrase,
                    StatusCode = (int)response.StatusCode,
                };

            var resultStr = await response.Content.ReadAsStringAsync();
            var wrapperModel = GetWrapperModel(resultStr);

            var model = new PasargadBaseRs<PasargadBankInternalPaymentRs>()
            {
                ErrorMessage = wrapperModel.ErrorMessage,
                ErrorCode = wrapperModel.ErrorCode,
                HasError = wrapperModel.HasError,
                MessageId = wrapperModel.MessageId,
                ReferenceNumber = wrapperModel.ReferenceNumber,
                StatusCode = wrapperModel.StatusCode,
                Count = wrapperModel.Count,
            };

            if (model.HasError)
                return model;

            var result = JsonHelper.GetItemByPathAsString(resultStr, "$.result.result");

            model.Result = new ResultInnerBase<PasargadBankInternalPaymentRs>();
            model.Result.IsSuccess = bool.Parse(JsonHelper.GetItemByPathAsString(result, "$.IsSuccess"));
            model.Result.Message = JsonHelper.GetItemByPathAsString(result, "$.Message");
            model.Result.RsCode = int.Parse(JsonHelper.GetItemByPathAsString(result, "$.RsCode"));
            //model.Result.UID = JsonHelper.GetItemByPathAsString(result, "$.UID");

            var jToken = JsonHelper.GetItemByPath(result, "$");
            model.Result.ResultData = new PasargadBankInternalPaymentRs()
            {
                Amount = request.SourceAmount,
                TransactionDate = jToken["Transactiondate"].ToString(),
                TransactionId = jToken["TransactionId"].ToString(),
                TransactionCode = jToken["TransactionCode"].ToString(),
            };

            return model;
        }

        public async Task<PasargadBaseRs<PasargadBankPayaPaymentRs>> PayaPayment(PasargadBankPayaPaymentRq request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_httpClient.BaseAddress, "/srv/sc/nzh/doServiceCall");

            if (_configuration.Banks.MockServer.IsEnabled)
                AddPostmanResponseNameToHeader(_httpClient, "PayaPayment");

            request.Description = GetFormattedDescription(request.Description);

            var collection = new List<KeyValuePair<string, string>>
            {
                new("scProductId", $"{_configuration.Banks.Pasargad.ProductIdPaya}"),
                new("scApiKey", $"{request.ScApiKeyPaya}"),
                new("request", request.ToJsonString()),
            };
            var content = new FormUrlEncodedContent(collection);

            var response = await _httpClient.PostAsync(uri, content, cancellationToken: cancellationToken);

            if (!response.IsSuccessStatusCode)
                return new PasargadBaseRs<PasargadBankPayaPaymentRs>()
                {
                    HasError = true,
                    ErrorCode = 0,
                    ErrorMessage = response.ReasonPhrase,
                    StatusCode = (int)response.StatusCode,
                };

            var resultStr = await response.Content.ReadAsStringAsync();
            var wrapperModel = GetWrapperModel(resultStr);

            var model = new PasargadBaseRs<PasargadBankPayaPaymentRs>()
            {
                ErrorMessage = wrapperModel.ErrorMessage,
                ErrorCode = wrapperModel.ErrorCode,
                HasError = wrapperModel.HasError,
                MessageId = wrapperModel.MessageId,
                ReferenceNumber = wrapperModel.ReferenceNumber,
                StatusCode = wrapperModel.StatusCode,
                Count = wrapperModel.Count,
            };

            if (model.HasError)
                return model;

            var result = JsonHelper.GetItemByPathAsString(resultStr, "$.result.result");

            model.Result = new ResultInnerBase<PasargadBankPayaPaymentRs>();
            model.Result.IsSuccess = bool.Parse(JsonHelper.GetItemByPathAsString(result, "$.IsSuccess"));
            model.Result.Message = JsonHelper.GetItemByPathAsString(result, "$.Message");
            model.Result.RsCode = int.Parse(JsonHelper.GetItemByPathAsString(result, "$.RsCode"));
            if (!model.Result.IsSuccess)
                return model;

            var jToken = JsonHelper.GetItemByPath(result, "$");
            model.Result.ResultData = new PasargadBankPayaPaymentRs()
            {
                Amount = double.Parse(jToken["Amount"].ToString()),
                RecieverFullName = jToken["RecieverFullNam"].ToString(),
                DestinationIban = jToken["DestinationIban"].ToString(),
                Description = jToken["Description"].ToString(),
                TransactionDate = jToken["TransactionDate"].ToString(),
                TransactionId = jToken["TransactionId"].ToString(),
                EndToEndId = jToken["EndToEndId"].ToString(),
                TransactionCode = jToken["TransactionCode"].ToString(),

            };

            return model;
        }

        public async Task<PasargadBaseRs<PasargadBankAccountBalanceRs>> GetAccountBalance(PasargadBankAccountBalanceRq request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_httpClient.BaseAddress, "/srv/sc/nzh/doServiceCall");

            if (_configuration.Banks.MockServer.IsEnabled)
                AddPostmanResponseNameToHeader(_httpClient, "AccountBalance");

            var collection = new List<KeyValuePair<string, string>>
            {
                new("scProductId", $"{_configuration.Banks.Pasargad.ProductIdAccountBalance}"),
                new("scApiKey", $"{request.ScApiKeyAccountBalance}"),
                new("request", $"{{\"DepositNumber\":\"{request.DepositNumber}\"}}"),
            };
            var content = new FormUrlEncodedContent(collection);

            var response = await _httpClient.PostAsync(uri, content, cancellationToken: cancellationToken);

            if (!response.IsSuccessStatusCode)
                return new PasargadBaseRs<PasargadBankAccountBalanceRs>()
                {
                    HasError = true,
                    ErrorCode = 0,
                    ErrorMessage = response.ReasonPhrase,
                    StatusCode = (int)response.StatusCode,
                };

            var resultStr = await response.Content.ReadAsStringAsync();
            var result = JsonHelper.GetItemByPathAsString(resultStr, "$.result.result");

            var wrapperModel = GetWrapperModel(resultStr);
            var model = new PasargadBaseRs<PasargadBankAccountBalanceRs>()
            {
                ErrorMessage = wrapperModel.ErrorMessage,
                ErrorCode = wrapperModel.ErrorCode,
                HasError = wrapperModel.HasError,
                MessageId = wrapperModel.MessageId,
                ReferenceNumber = wrapperModel.ReferenceNumber,
                StatusCode = wrapperModel.StatusCode,
                Count = wrapperModel.Count,
            };

            if (model.HasError)
                return model;

            model.Result = new ResultInnerBase<PasargadBankAccountBalanceRs>();
            model.Result.IsSuccess = bool.Parse(JsonHelper.GetItemByPathAsString(result, "$.IsSuccess"));
            model.Result.Message = JsonHelper.GetItemByPathAsString(result, "$.Message");
            model.Result.RsCode = int.Parse(JsonHelper.GetItemByPathAsString(result, "$.RsCode"));
            if(!model.Result.IsSuccess)
                return model;

            var jToken = JsonHelper.GetItemByPath(result, "$.ResultData");
            model.Result.ResultData = new PasargadBankAccountBalanceRs()
            {
                DepositAvailableBalance = double.Parse(jToken["DepositAvailableBalance"].ToString()),
                DepositBalance = double.Parse(jToken["DepositBalance"].ToString()),
                DepositNumber = jToken["depositNumber"].ToString(),
                UID = JsonHelper.GetItemByPathAsString(result, "$.UID"),
            };

            return model;
        }

        public async Task<PasargadBaseRs<PasargadBankSatnaPaymentRs>> SatnaPayment(PasargadBankSatnaPaymentRq request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_httpClient.BaseAddress, "/srv/sc/nzh/doServiceCall");

            if (_configuration.Banks.MockServer.IsEnabled)
                AddPostmanResponseNameToHeader(_httpClient, "SatnaPayment");

            request.Description = GetFormattedDescription(request.Description);

            var collection = new List<KeyValuePair<string, string>>
            {
                new("scProductId", $"{_configuration.Banks.Pasargad.ProductIdSatna}"),
                new("scApiKey", $"{request.ScApiKeySatna}"),
                new("request", request.ToJsonString()),
            };
            var content = new FormUrlEncodedContent(collection);

            var response = await _httpClient.PostAsync(uri, content, cancellationToken: cancellationToken);

            if (!response.IsSuccessStatusCode)
                return new PasargadBaseRs<PasargadBankSatnaPaymentRs>()
                {
                    HasError = true,
                    ErrorCode = 0,
                    ErrorMessage = response.ReasonPhrase,
                    StatusCode = (int)response.StatusCode,
                };

            var resultStr = await response.Content.ReadAsStringAsync();
            var wrapperModel = GetWrapperModel(resultStr);

            var model = new PasargadBaseRs<PasargadBankSatnaPaymentRs>()
            {
                ErrorMessage = wrapperModel.ErrorMessage,
                ErrorCode = wrapperModel.ErrorCode,
                HasError = wrapperModel.HasError,
                MessageId = wrapperModel.MessageId,
                ReferenceNumber = wrapperModel.ReferenceNumber,
                StatusCode = wrapperModel.StatusCode,
                Count = wrapperModel.Count,
            };

            if (model.HasError)
                return model;

            var result = JsonHelper.GetItemByPathAsString(resultStr, "$.result.result");

            model.Result = new ResultInnerBase<PasargadBankSatnaPaymentRs>();
            model.Result.IsSuccess = bool.Parse(JsonHelper.GetItemByPathAsString(result, "$.IsSuccess"));
            model.Result.Message = JsonHelper.GetItemByPathAsString(result, "$.Message");
            model.Result.RsCode = int.Parse(JsonHelper.GetItemByPathAsString(result, "$.RsCode"));
            if (!model.Result.IsSuccess)
                return model;

            var jToken = JsonHelper.GetItemByPath(result, "$");
            model.Result.ResultData = new PasargadBankSatnaPaymentRs()
            {
                Amount = double.Parse(jToken["Amount"].ToString()),
                RecieverFullName = $"{jToken["RecieverName"]} {jToken["RecieverLastName"]}",
                DestionationIban = jToken["DestinationDepNum"].ToString(),
                Description = $"UserReferenceNumber: {jToken["UserReferenceNumber"]}",
                TransactionDate = jToken["TransactionDate"].ToString(),
                TransactionId = jToken["TransactionId"].ToString(),
                TransactionCode = jToken["TransactionCode"].ToString(),

            };

            return model;
        }
        
        public async Task<PasargadBaseRs<PasargadBankKYCRs>> IsKYCCompliant(PasargadBankKYCRq request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_httpClient.BaseAddress, "/srv/sc/nzh/doServiceCall");

            if (_configuration.Banks.MockServer.IsEnabled)
                AddPostmanResponseNameToHeader(_httpClient, "KYCCompatible");

            request.Iban = request.Iban.Replace("-", "");

            var collection = new List<KeyValuePair<string, string>>
            {
                new("scProductId", $"{_configuration.Banks.Pasargad.ProductIdKYC}"),
                new("scApiKey", $"{request.ScApiKeyKYC}"),
                new("iban", request.Iban),
                new("nationalCode", request.NationalCode),
                new("birthDate", request.BirthDate),
            };
            var content = new FormUrlEncodedContent(collection);

            var response = await _httpClient.PostAsync(uri, content, cancellationToken: cancellationToken);

            if (!response.IsSuccessStatusCode)
                return new PasargadBaseRs<PasargadBankKYCRs>()
                {
                    HasError = true,
                    ErrorCode = 0,
                    ErrorMessage = response.ReasonPhrase,
                    StatusCode = (int)response.StatusCode,
                };

            var resultStr = await response.Content.ReadAsStringAsync();
            var result = JsonHelper.GetItemByPathAsString(resultStr, "$.result.result");

            var wrapperModel = GetWrapperModel(resultStr);
            var model = new PasargadBaseRs<PasargadBankKYCRs>()
            {
                ErrorMessage = wrapperModel.ErrorMessage,
                ErrorCode = wrapperModel.ErrorCode,
                HasError = wrapperModel.HasError,
                MessageId = wrapperModel.MessageId,
                ReferenceNumber = wrapperModel.ReferenceNumber,
                StatusCode = wrapperModel.StatusCode,
                Count = wrapperModel.Count,
            };

            if (model.HasError)
                return model;

            model.Result = new ResultInnerBase<PasargadBankKYCRs>();
            model.Result.IsSuccess = true;
            model.Result.Message = "";
            model.Result.RsCode = 0;

            var jToken = JsonHelper.GetItemByPath(result, "$");
            model.Result.ResultData = new PasargadBankKYCRs()
            {
                Matched = bool.Parse(jToken["matched"].ToString()),
            };

            return model;
        }

        protected string GetFormattedDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return "";

            var linesList = new List<string>();
            var startingIndex = 0;
            var numberOfLineChars = 33;
            var lineSeparator = "#";

            do
            {
                if (string.IsNullOrWhiteSpace(description)) break;

                if (description.Length > numberOfLineChars)
                {
                    linesList.Add(description.Substring(startingIndex, numberOfLineChars));
                    description = description.Remove(startingIndex, numberOfLineChars);
                }
                else
                {
                    linesList.Add(description);
                    description = "";
                }
            } while (!string.IsNullOrWhiteSpace(description) || linesList.Count < 6);

            return string.Join(lineSeparator, linesList);
        }

        private PasargadBaseRs<object> GetWrapperModel(string jsonData) =>
            jsonData.FromJsonString<PasargadBaseRs<object>>();

        private void AddPostmanResponseNameToHeader(HttpClient httpClient, string name)
        {
            if (httpClient.DefaultRequestHeaders.Contains(GlobalConstants.PostmanMockResponseKey))
                httpClient.DefaultRequestHeaders.Remove(GlobalConstants.PostmanMockResponseKey);
            httpClient.DefaultRequestHeaders.Add(GlobalConstants.PostmanMockResponseKey, name);
        }

    }
}
