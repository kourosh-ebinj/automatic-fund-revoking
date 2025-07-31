using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Requests.ThirdParties.Pasargad;
using Application.Models.Responses.ThirdParties.Pasargad;
using Application.Services.Abstractions.HttpClients.ThirdParties.BankingProviders;
using Application.Services.Abstractions.ThirdParties.Banks;
using Microsoft.Extensions.Logging;
using Core.Extensions;
using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Enums;
using Application.Enums;
using Application.Services.Abstractions;
using Application.Services.Abstractions.Caching;

namespace Infrastructure.Services.EntityFramework.ThirdParties.Banks
{
    public class PasargadBankingProviderService : BankingProviderService, IPasargadBankingProviderService
    {
        private readonly IPasargadBankClientService _pasargadBankClientService;
        private readonly IIBanKYCService _iIBanKYCService;
        private readonly IIBanKYCCacheService _iIBanKYCCacheService;
        private readonly IPasargadBankAccountDetailService _pasargadBankAccountDetailService;

        public PasargadBankingProviderService(IPasargadBankClientService pasargadBankClientService, IIBanKYCService iIBanKYCService,
                                              IIBanKYCCacheService iIBanKYCCacheService,
                                              IPasargadBankAccountDetailService pasargadBankAccountDetailService)
        {
            _pasargadBankClientService = pasargadBankClientService;
            _iIBanKYCService = iIBanKYCService;
            _iIBanKYCCacheService = iIBanKYCCacheService;
            _pasargadBankAccountDetailService = pasargadBankAccountDetailService;
        }

        public override async Task<double> GetAccountBalance(BankAccountRs bankAccount, CancellationToken cancellationToken = default)
        {
            var pasargadBankAccountTokens = await _pasargadBankAccountDetailService.GetByBankAccountId(bankAccount.Id);
            _guard.Assert(pasargadBankAccountTokens is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $"شناسه حساب بانکی مبدا یافت نشد.");

            var accountBalanceModel = new PasargadBankAccountBalanceRq()
            {
                ScApiKeyAccountBalance = pasargadBankAccountTokens.ScApiKeyAccountBalance,
                DepositNumber = bankAccount.AccountNumber,
            };

            var result = await GetAccountBalance(accountBalanceModel, cancellationToken);

            if (result.HasError)
            {
                var logData = new
                {
                    accountBalanceModel,
                    result
                };
                _logger.LogWarning(logData);
                return default;
            }

            _guard.Assert(result.Result.IsSuccess, Core.Enums.ExceptionCodeEnum.BadRequest, result.Result.Message);
            return result.Result.ResultData.DepositAvailableBalance;
        }

        public override async Task<BankPaymentMethodResultRs> InternalPayment(BankInternalPaymentRq request, CancellationToken cancellationToken = default)
        {
            var pasargadBankAccountTokens = await _pasargadBankAccountDetailService.GetByBankAccountId(request.SourceBankAccount.Id);
            _guard.Assert(pasargadBankAccountTokens is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $"شناسه حساب بانکی مبدا یافت نشد.");

            var paymentModel = new PasargadBankInternalPaymentRq()
            {
                //TransactionId = GetNewTransactionId(_applicationSetting.Banks.Pasargad.OrgCodePaya),
                ScApiKeyInternal = pasargadBankAccountTokens.ScApiKeyInternal,
                SourceAmount = request.Amount,
                SourceComment = request.SourceComment,
                SourceAccount = request.SourceBankAccount.AccountNumber,
                DocumentItemType = PasargadBankDocumentItemTypeEnum.Deposit,
                Creditors = [new Creditor()
                {
                    DestinationAccount = request.DestAccountNumber,
                    DestinationAmount = request.Amount,
                    DestinationComment = request.DestComment,
                    DocumentItemType = PasargadBankDocumentItemTypeEnum.Deposit,
                }],
                TransferBillNumber = "",
            };

            _guard.Assert(request.Amount >= 10000, Core.Enums.ExceptionCodeEnum.BadRequest, $"مبلغ نادرست است.");

            var result = await InternalPayment(paymentModel, cancellationToken);

            var logData = new { paymentModel, result };
            _logger.LogWarning(logData);

            if (result.HasError || !result.Result.IsSuccess)
            {
                return new BankPaymentMethodResultRs()
                {
                    PaymentStatus = TransactionStatusEnum.Failed,
                    Message = result.ErrorMessage,
                    MessageCode = result.MessageId.ToString(),
                };
            }

            return new BankPaymentMethodResultRs()
            {
                TransactionId = result.Result.ResultData.TransactionId,
                TransactionDate = result.Result.ResultData.TransactionDate,
                PaymentStatus = TransactionStatusEnum.Successfull,
                Message = result.Result.Message,
                MessageCode = result.Result.RsCode.ToString(),
            };
        }

        public override async Task<BankPaymentMethodResultRs> PayaPayment(BankPayaPaymentRq request, CancellationToken cancellationToken = default)
        {
            var pasargadBankAccountTokens = await _pasargadBankAccountDetailService.GetByBankAccountId(request.SourceBankAccount.Id);
            _guard.Assert(pasargadBankAccountTokens is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $"شناسه حساب بانکی مبدا یافت نشد.");

            var minAmount = _applicationSetting.Banks.Markazi.PayaMin;
            var maxAmount = _applicationSetting.Banks.Markazi.PayaMax;
            _guard.Assert(request.Amount >= minAmount, Core.Enums.ExceptionCodeEnum.BadRequest, $"حداقل مبلغ پایا {minAmount} ریال می باشد");
            _guard.Assert(request.Amount <= maxAmount, Core.Enums.ExceptionCodeEnum.BadRequest, $"حداکثر مبلغ پایا {maxAmount} ریال می باشد");

            _guard.Assert(!string.IsNullOrWhiteSpace(request.SourceAccountSheba), Core.Enums.ExceptionCodeEnum.BadRequest, $"شماره شبا مقصد نامعتبر است.");
            _guard.Assert(!string.IsNullOrWhiteSpace(request.DestNationalCode), Core.Enums.ExceptionCodeEnum.BadRequest, $"کد ملی مقصد نامعتبر است.");
            _guard.Assert(!string.IsNullOrWhiteSpace(request.DestAccountSheba), Core.Enums.ExceptionCodeEnum.BadRequest, $"شماره شبا مقصد نامعتبر است.");

            if (!await IsKYCCompliant(request.DestNationalCode, request.DestAccountSheba, pasargadBankAccountTokens.ScApiKeyKYC))
                return new BankPaymentMethodResultRs()
                {
                    PaymentStatus = TransactionStatusEnum.Failed,
                    Message = $"کد ملی با شماره شبا تطابق ندارد.",
                    MessageCode = "222",
                };

            var paymentModel = new PasargadBankPayaPaymentRq()
            {
                ScApiKeyPaya = pasargadBankAccountTokens.ScApiKeyPaya,
                Amount = request.Amount,
                SourceDepNum = request.SourceBankAccount.AccountNumber,
                SenderReturnDepositNumber = request.SourceBankAccount.AccountNumber,
                DestComment = request.DestComment,
                FullName = request.SourceFullName,
                DestinationIban = request.DestAccountSheba,
                TransactionId = GetNewTransactionId(_applicationSetting.Banks.Pasargad.OrgCodePaya),
                SrcComment = request.SourceComment,
                RecieverFullName = request.DestFullName,
                Description = request.DestComment,
                DetailType = ((int)request.DetailType).ToString(),
                RecieverNationalCode = request.DestNationalCode,

                //MobileNumber = request.MobileNumber,
                //CommissionDepositNumber = "",
                //CustomerNumber = "",
                //DestBankCode = "",
                //Nationality = "",
                //Address = "",
                //PhoneNumber = "",
                //PostalCode = "",
                //ShahabCode = "",
                //SourceTMBillNumber = "",
                //TransactionBillNumber = "",
            };

            var result = await PayaPayment(paymentModel, cancellationToken);

            var logData = new { paymentModel, result };
            _logger.LogWarning(logData);

            if (result.HasError || !result.Result.IsSuccess)
            {
                return new BankPaymentMethodResultRs()
                {
                    PaymentStatus = TransactionStatusEnum.Failed,
                    Message = result.ErrorMessage,
                    MessageCode = result.MessageId.ToString(),
                };
            }

            return new BankPaymentMethodResultRs()
            {
                TransactionId = result.Result.ResultData.TransactionId,
                TransactionDate = result.Result.ResultData.TransactionDate,
                PaymentStatus = TransactionStatusEnum.Successfull,
                Message = result.Result.Message,
                MessageCode = result.Result.RsCode.ToString(),
            };
        }

        public override async Task<BankPaymentMethodResultRs> SatnaPayment(BankSatnaPaymentRq request, CancellationToken cancellationToken = default)
        {
            var pasargadBankAccountTokens = await _pasargadBankAccountDetailService.GetByBankAccountId(request.SourceBankAccount.Id);
            _guard.Assert(pasargadBankAccountTokens is not null, Core.Enums.ExceptionCodeEnum.BadRequest, $"شناسه حساب بانکی مبدا یافت نشد.");


            var minAmount = _applicationSetting.Banks.Markazi.SatnaMin;
            var maxAmount = _applicationSetting.Banks.Markazi.SatnaMax;
            _guard.Assert(request.Amount >= minAmount, Core.Enums.ExceptionCodeEnum.BadRequest, $"حداقل مبلغ ساتنا {minAmount} ریال می باشد");
            _guard.Assert(request.Amount <= maxAmount, Core.Enums.ExceptionCodeEnum.BadRequest, $"حداکثر مبلغ ساتنا {maxAmount} ریال می باشد");

            _guard.Assert(!string.IsNullOrWhiteSpace(request.SourceAccountSheba), Core.Enums.ExceptionCodeEnum.BadRequest, $"شماره شبا مقصد نامعتبر است.");
            _guard.Assert(!string.IsNullOrWhiteSpace(request.DestNationalCode), Core.Enums.ExceptionCodeEnum.BadRequest, $"کد ملی مقصد نامعتبر است.");
            _guard.Assert(!string.IsNullOrWhiteSpace(request.DestAccountSheba), Core.Enums.ExceptionCodeEnum.BadRequest, $"شماره شبا مقصد نامعتبر است.");

            if (!await IsKYCCompliant(request.DestNationalCode, request.DestAccountSheba, pasargadBankAccountTokens.ScApiKeyKYC))
                return new BankPaymentMethodResultRs()
                {
                    PaymentStatus = TransactionStatusEnum.Failed,
                    Message = $"کد ملی با شماره شبا تطابق ندارد.",
                    MessageCode = "222",
                };

            var paymentModel = new PasargadBankSatnaPaymentRq()
            {
                ScApiKeySatna = pasargadBankAccountTokens.ScApiKeySatna,
                Amount = request.Amount,
                SourceDepNum = request.SourceBankAccount.AccountNumber,
                SenderReturnDepositNumber = request.SourceBankAccount.AccountNumber,
                DestComment = request.Description,
                RecieverLastName = request.DestFullName,
                TransactionId = GetNewTransactionId(_applicationSetting.Banks.Pasargad.OrgCodePaya),
                SrcComment = "",
                RecieverName = request.DestFullName,
                SenderFamilyNameOrCompanyName = request.DestFullName,
                DetailType = request.DetailType,
                //natio = request.NationalCode,
                //MobileNumber = request.MobileNumber,
                //CommissionDepositNumber = "",
                //CustomerNumber = "",
                //DestBankCode = "",
                //Nationality = "",
                //Address = "",
                //PhoneNumber = "",
                //PostalCode = "",
                //ShahabCode = "",
                //SourceTMBillNumber = "",
                //TransactionBillNumber = "",
            };

            var result = await SatnaPayment(paymentModel, cancellationToken);

            var logData = new { paymentModel, result };
            _logger.LogWarning(logData);

            if (result.HasError || !result.Result.IsSuccess)
            {
                return new BankPaymentMethodResultRs()
                {
                    PaymentStatus = TransactionStatusEnum.Failed,
                    Message = result.ErrorMessage,
                    MessageCode = result.MessageId.ToString(),
                };
            }

            return new BankPaymentMethodResultRs()
            {
                TransactionId = result.Result.ResultData.TransactionId,
                TransactionDate = result.Result.ResultData.TransactionDate,
                PaymentStatus = TransactionStatusEnum.Successfull,
                Message = result.Result.Message,
                MessageCode = result.Result.RsCode.ToString(),
            };
        }

        public async Task<PasargadBaseRs<PasargadBankAccountBalanceRs>> GetAccountBalance(PasargadBankAccountBalanceRq request, CancellationToken cancellationToken = default)
        {

            return await _pasargadBankClientService.GetAccountBalance(request, cancellationToken);
        }

        public async Task<PasargadBaseRs<PasargadBankInternalPaymentRs>> InternalPayment(PasargadBankInternalPaymentRq request, CancellationToken cancellationToken = default)
        {

            return await _pasargadBankClientService.InternalPayment(request, cancellationToken);
        }

        public async Task<PasargadBaseRs<PasargadBankPayaPaymentRs>> PayaPayment(PasargadBankPayaPaymentRq request, CancellationToken cancellationToken = default)
        {

            return await _pasargadBankClientService.PayaPayment(request, cancellationToken);
        }

        public async Task<PasargadBaseRs<PasargadBankSatnaPaymentRs>> SatnaPayment(PasargadBankSatnaPaymentRq request, CancellationToken cancellationToken = default)
        {

            return await _pasargadBankClientService.SatnaPayment(request, cancellationToken);
        }

        public async Task<PasargadBaseRs<PasargadBankKYCRs>> IsKYCCompliant(PasargadBankKYCRq request, CancellationToken cancellationToken = default)
        {
            
            return await _pasargadBankClientService.IsKYCCompliant(request, cancellationToken);
        }

        private string GetNewTransactionId(int orgCode)
        {

            var randomString = _applicationSetting.Banks.Pasargad.RandomString;
            //var epoch = DateTime.UnixEpoch; // for Epoch timestamp style
            var dateTime = GetDateTime(DateTime.Now); //Date Time
            var jsAsciiCode = 0;

            var transactionId = orgCode + randomString + dateTime;
            foreach (var character in transactionId.ToArray())
            {
                if (character != '-')
                    jsAsciiCode += Convert.ToInt16(character);
            };

            return $"{orgCode}-{randomString}-{dateTime}-{jsAsciiCode}";
        }

        // 2024 03 11 11 11 53 569
        private string GetDateTime(DateTime dateTime) =>
            dateTime.ToDateTimeString("yyyyMMddHHmmssFFF");

        private async Task<bool> IsKYCCompliant(string nationalCode, string iban, string scApiKeyKYC)
        {
            var ibanKYC = await _iIBanKYCService.GetByIBan(iban);
            if (ibanKYC is not null)
                return ibanKYC.IsKYCCompliant;

            var isKYCCompatibleResult = await IsKYCCompliant(new PasargadBankKYCRq()
            {
                ScApiKeyKYC = scApiKeyKYC,
                BirthDate = "14030230",
                NationalCode = nationalCode,
                Iban = iban,
            });

            _guard.Assert(!isKYCCompatibleResult.HasError, Core.Enums.ExceptionCodeEnum.BadRequest,
                    $"KYC Error for nationalcode ({nationalCode}): {isKYCCompatibleResult.ErrorMessage} ({isKYCCompatibleResult.ErrorCode})",
                    nationalCode, iban);

            var isCompliant = isKYCCompatibleResult.Result.IsSuccess &&
                               isKYCCompatibleResult.Result.ResultData.Matched;
            await _iIBanKYCService.Create(iban, isCompliant);

            return isCompliant;
        }
    }
}
