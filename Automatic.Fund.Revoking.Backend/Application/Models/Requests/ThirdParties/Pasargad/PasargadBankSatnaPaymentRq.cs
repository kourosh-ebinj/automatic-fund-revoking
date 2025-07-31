using System;
using System.Collections;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Application.Enums;

namespace Application.Models.Requests.ThirdParties.Pasargad
{
    public record PasargadBankSatnaPaymentRq : PasargadBankPaymentBaseRq
    {
        public string ScApiKeySatna { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public PasargadBankPayaDetailTypeEnum DetailType { get; set; }

        public string Description { get; set; }

        public double Amount { get; set; }

        [Description("شماره سپرده­ی مبدا")]
        public string SourceDepNum { get; set; }

        [Description("شماره شبای مقصد")]
        public string DestinationDepNum { get; set; }

        [Description("نام گیرنده‌ی وجه")]
        public string RecieverName { get; set; }
        
        [Description("نام خانوادگی گیرنده‌ی وجه")]
        public string RecieverLastName { get; set; }

        public string SrcComment { get; set; }
        public string DestComment { get; set; }
        public string SenderNationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }

        [Description("شناسه‌ی واریز")]
        public string TransactionBillNumber { get; set; }

        [Description("شماره سپرده‌ی بازگشت وجه")]
        public string SenderReturnDepositNumber ;

        [Description("شماره مشتری واریزکننده")]
        public string SenderCustomerNumber { get; set; }

        [Description("سپرده‌ ای که  کارمزد از آن برداشته میشود")]
        public string CommissionDepositNumber { get; set; }

        [Description("کد بانک مقصد")]
        public string DestBankCode { get; set; }

        [Description("شناسه قبض عملیات مالی.")]
        public string SourceTMBillNumber { get; set; }

        [Description("کد شهاب")]
        public string SenderShahabCode { get; set; }

        public string SenderPostalCode { get; set; }
        public string Nationality { get; set; }

        [Description("نام فرستنده")]
        public string SenderNameOrCompanyType { get; set; }

        [Description("نام خانوادگی فرستنده")]
        public string SenderFamilyNameOrCompanyName { get; set; }

    }
}
