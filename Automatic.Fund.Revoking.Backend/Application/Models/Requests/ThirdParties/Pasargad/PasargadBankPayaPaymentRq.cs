using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Serialization;
using Application.Enums;

namespace Application.Models.Requests.ThirdParties.Pasargad
{
    public record PasargadBankPayaPaymentRq : PasargadBankPaymentBaseRq
    {

        public string ScApiKeyPaya { get; set; }
        public string DetailType { get; set; }

        public string Description { get; set; }

        public double Amount { get; set; }

        [Description("شماره سپرده­ی مبدا")]
        public string SourceDepNum { get; set; }

        [Description("شماره شبای مقصد")]
        public string DestinationIban { get; set; }

        public string RecieverFullName { get; set; }
        
        public string SrcComment { get; set; }

        public string DestComment { get; set; }

        public string RecieverNationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }

        [Description("شناسه‌ی واریز")]
        public string TransactionBillNumber { get; set; }

        [Description("شماره سپرده‌ی بازگشت وجه")]
        public string SenderReturnDepositNumber;

        [Description("شماره مشتری")]
        public string CustomerNumber { get; set; }

        [Description("سپرده‌ی کارمزد")]
        public string CommissionDepositNumber { get; set; }

        [Description("کد بانک مقصد")]
        public string DestBankCode { get; set; }

        [Description("شناسه قبض عملیات مالی.")]
        public string SourceTMBillNumber { get; set; }

        [Description("کد شهاب")]
        public string ShahabCode { get; set; }
        
        public string PostalCode { get; set; }
        public string Nationality { get; set; }
        public string FullName { get; set; }

    }
}
