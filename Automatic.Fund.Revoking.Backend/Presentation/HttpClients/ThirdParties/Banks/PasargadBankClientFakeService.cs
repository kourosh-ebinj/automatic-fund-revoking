using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Requests.ThirdParties.Pasargad;
using Application.Models.Responses.ThirdParties.Pasargad;
using Application.Services.Abstractions.HttpClients.ThirdParties.BankingProviders;
using Core.Extensions;
using Core.Helpers;
using WebCore.Services.HttpClients;

namespace Presentation.HttpClients.ThirdParties.Banks
{
    public class PasargadBankClientFakeService : HttpClientService<PasargadBankClientFakeService>, IPasargadBankClientService
    {
        private readonly ApplicationSettingExtenderModel _configuration;

        public PasargadBankClientFakeService(HttpClient httpClient, ApplicationSettingExtenderModel configuration) : base(httpClient)
        {
            _configuration = configuration;

        }

        public async Task<PasargadBaseRs<PasargadBankInternalPaymentRs>> InternalPayment(PasargadBankInternalPaymentRq request, CancellationToken cancellationToken = default)
        {
            var model = new PasargadBaseRs<PasargadBankInternalPaymentRs>()
            {
                MessageId = 1,
                ReferenceNumber = "987654321",
                StatusCode = 200,
            };
            model.Result = new ResultInnerBase<PasargadBankInternalPaymentRs>()
            {
                IsSuccess = true,
                Message = "درخواست با موفقیت انجام شد.",
                RsCode = 1,
                ResultData = new PasargadBankInternalPaymentRs()
                {
                    Amount = request.SourceAmount,
                    TransactionId = request.TransactionId,
                },

            };

            Console.WriteLine($"**Fake** Internal: {model.Result.ResultData.Amount} paid to Account No ({request.Creditors.First().DestinationAccount}). Transaction: {model.Result.ResultData.TransactionId}");

            return await Task.FromResult(model);
        }

        public async Task<PasargadBaseRs<PasargadBankPayaPaymentRs>> PayaPayment(PasargadBankPayaPaymentRq request, CancellationToken cancellationToken = default)
        {
            var model = new PasargadBaseRs<PasargadBankPayaPaymentRs>()
            {
                MessageId = 1,
                ReferenceNumber = "123456789",
                StatusCode = 200,
            };
            model.Result = new ResultInnerBase<PasargadBankPayaPaymentRs>()
            {
                IsSuccess = true,
                Message = "درخواست با موفقیت انجام شد.",
                RsCode = 1,
                ResultData = new PasargadBankPayaPaymentRs()
                {
                    Amount = request.Amount,
                    Description = "درخواست با موفقیت انجام شد.",
                    EndToEndId = "24556785123534456",
                    RecieverFullName = request.RecieverFullName,
                    TransactionCode = "11212",
                    TransactionId = request.TransactionId,
                },

            };

            Console.WriteLine($"**Fake** Paya: {model.Result.ResultData.Amount} paid to {request.FullName}. Transaction: {model.Result.ResultData.TransactionId}");

            return await Task.FromResult(model);
        }

        public async Task<PasargadBaseRs<PasargadBankAccountBalanceRs>> GetAccountBalance(PasargadBankAccountBalanceRq request, CancellationToken cancellationToken = default)
        {
            var model = new PasargadBaseRs<PasargadBankAccountBalanceRs>()
            {
                ReferenceNumber = "1234567890",
                StatusCode = 200,
            };

            model.Result = new ResultInnerBase<PasargadBankAccountBalanceRs>();
            model.Result.IsSuccess = true;
            model.Result.Message = "با موفقیت انجام شد";
            model.Result.RsCode = 122;
            
            model.Result.ResultData = new PasargadBankAccountBalanceRs()
            {
                DepositAvailableBalance = 999999999,
                DepositBalance = 999999999,
                DepositNumber = request.DepositNumber,
                UID = Guid.NewGuid().ToString(),
            };

            Console.WriteLine($"**Fake** Account Balance: {request.DepositNumber} = {model.Result.ResultData.DepositAvailableBalance}");

            return await Task.FromResult(model);
        }

        public async Task<PasargadBaseRs<PasargadBankSatnaPaymentRs>> SatnaPayment(PasargadBankSatnaPaymentRq request, CancellationToken cancellationToken = default)
        {
            var model = new PasargadBaseRs<PasargadBankSatnaPaymentRs>()
            {
                MessageId = 1,
                ReferenceNumber = "123456789",
                StatusCode = 200,
            };
            model.Result = new ResultInnerBase<PasargadBankSatnaPaymentRs>()
            {
                IsSuccess = true,
                Message = "درخواست با موفقیت انجام شد.",
                RsCode = 1,
                ResultData = new PasargadBankSatnaPaymentRs()
                {
                    Amount = request.Amount,
                    Description = "درخواست با موفقیت انجام شد.",
                    RecieverFullName = request.RecieverLastName,
                    TransactionCode = "11212",
                    TransactionId = request.TransactionId,
                },

            };

            Console.WriteLine($"**Fake** Satna: {model.Result.ResultData.Amount} paid to {request.RecieverLastName} . Transaction: {model.Result.ResultData.TransactionId}");

            return await Task.FromResult(model);
        }

        public async Task<PasargadBaseRs<PasargadBankKYCRs>> IsKYCCompliant(PasargadBankKYCRq request, CancellationToken cancellationToken = default)
        {
            var model = new PasargadBaseRs<PasargadBankKYCRs>()
            {
                MessageId = 1,
                ReferenceNumber = "123456789",
                StatusCode = 200,
            };
            model.Result = new ResultInnerBase<PasargadBankKYCRs>()
            {
                IsSuccess = true,
                Message = "درخواست با موفقیت انجام شد.",
                RsCode = 1,
                ResultData = new PasargadBankKYCRs()
                {
                    Matched = true,
                },

            };

            Console.WriteLine($"**Fake** KYC: Sheba: {request.Iban}, NationalCode: {request.NationalCode} :  {model.Result.ResultData.Matched} .");

            return await Task.FromResult(model);
        }


        //public async Task<PasargadBaseRs<PasargadBankAccountNumberRs>> GetAccountNumber(PasargadBankAccountNumberRq request, CancellationToken cancellationToken = default)

        //{
        //    var model = new PasargadBaseRs<PasargadBankAccountNumberRs>()
        //    {
        //        ReferenceNumber = "1234567890",
        //        StatusCode = 200,
        //    };

        //    model.Result = new ResultInnerBase<PasargadBankAccountNumberRs>();
        //    model.Result.IsSuccess = true;
        //    model.Result.Message = "با موفقیت انجام شد";
        //    model.Result.RsCode = 122;

        //    model.Result.ResultData = new PasargadBankAccountNumberRs()
        //    {
        //        DepositNumber = "203.110.1419864.1",
        //    };

        //    Console.WriteLine($"**Fake** Account Number: {request.ShebaNumber} = {model.Result.ResultData.DepositNumber}");

        //    return await Task.FromResult(model);
        //}


    }
}
