using System.ComponentModel;
using System.Text.Json.Serialization;
using Application.Models.Requests.ThirdParties.Pasargad;

namespace Application.Models.Responses.ThirdParties.Pasargad
{
    public record PasargadBankInternalPaymentRs
    {
        [Description("شناسه یکتای تراکنش")]
        public string TransactionId { get; set; }

        [Description("تاریخ انجام تراکنش")]
        public string TransactionDate { get; set; }
        public string TransactionCode { get; set; }

        [Description("مبلغ وجه واریزی")]
        public double Amount { get; set; }

    }

}
