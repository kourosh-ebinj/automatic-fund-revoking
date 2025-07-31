using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Application.Models.Responses.ThirdParties.Pasargad
{
    public record PasargadBankSatnaPaymentRs
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
        public string DestionationIban   { get; set; }
        
        public string Description { get; set; }

        [Description("شماره پیگیری تراکنش")]
        public string TransactionCode { get; set; }


    }
}
