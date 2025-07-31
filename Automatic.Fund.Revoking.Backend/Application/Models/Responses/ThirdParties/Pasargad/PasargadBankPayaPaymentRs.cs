using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Application.Models.Responses.ThirdParties.Pasargad
{
    public record PasargadBankPayaPaymentRs 
    {
        [Description("شناسه یکتای تراکنش")]
        public string TransactionId { get; set; }
        
        [Description("تاریخ انجام تراکنش")]
        public string TransactionDate { get; set; }
        
        [Description("مبلغ وجه واریزی")]
        public double Amount { get; set; }
        
        [Description("نام مشتری مقصد")]
        public string RecieverFullName { get; set; }
        
        [Description("شماره شبای مشتری مقصد")]
        public string DestinationIban   { get; set; }
        
        public string Description { get; set; }

        [Description("شناسه یکتای دریافت شده از core که جهت تایید درخواست انتقال وجه پایا از آن استفاده می‌شود.")]
        public string EndToEndId { get; set; }
        
        [Description("شماره پیگیری تراکنش")]
        public string TransactionCode { get; set; }


    }
}
