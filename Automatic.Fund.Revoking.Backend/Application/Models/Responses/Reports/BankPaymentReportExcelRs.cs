using System;
using System.ComponentModel;
using Core.Models.Responses;
using Domain.Enums;

namespace Application.Models.Responses.Reports
{
    public record BankPaymentReportExcelRs
    {
        [Description("ردیف")]
        public int Index { get; set; }

        [Description("شناسه سفارش")]
        public long OrderId { get; set; }

        [Description("نام بانک مبدا")]
        public string SourceBankName { get; set; }

        [Description("شماره حساب مبدا")]
        public string SourceBankAccountNumber { get; set; }

        [Description("شماره شبای مقصد")]
        public string DestinationShebaNumber { get; set; }

        [Description("نام بانک مقصد")]
        public string DestinationBankName { get; set; }

        [Description("جمع کل (ریال)")]
        public string TotalAmount { get; set; }

        [Description("شناسه یکتای پرداخت")]
        public string BankUniqueId { get; set; }

        [Description("توضیحات")]
        public string Description { get; set; }

        [Description("روش پرداخت")]
        public string BankPaymentMethod { get; set; }

        [Description("وضعیت تراکنش")]
        public string TransactionStatus { get; set; }

    }
}
