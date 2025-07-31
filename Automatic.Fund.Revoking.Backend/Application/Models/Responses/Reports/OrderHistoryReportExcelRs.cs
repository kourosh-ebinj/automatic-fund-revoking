using System;
using System.ComponentModel;
using Core.Models.Responses;
using Domain.Enums;

namespace Application.Models.Responses.Reports
{
    public record OrderHistoryReportExcelRs
    {
        [Description("ردیف")]
        public int Index { get; set; }

        [Description("نام صندوق")]
        public string FundName { get; set; }

        [Description("عنوان سفارش")]
        public string Title { get; set; }

        [Description("جمع کل (ریال)")]
        public string TotalAmount { get; set; }

        [Description("تعداد واحد ها")]
        public int TotalUnits { get; set; }

        [Description("وضعیت سفارش")]
        public string OrderStatus { get; set; }

        [Description("توضیحات وضعیت سفارش")]
        public string OrderStatusDescription { get; set; }

        [Description("کد یکتای سفارش در سیستم خارجی")]
        public string RayanFundOrderId { get; set; }

        [Description("نام برنامه ای که سفارش در آن ثبت شده است")]
        public string AppName { get; set; }

        [Description("شناسه مشتری")]
        public string CustomerId { get; set; }

        [Description("نام و نام خانوادگی مشتری")]
        public string CustomerFullName { get; set; }

        [Description("کد ملی مشتری")]
        public string CustomerNationalCode { get; set; }

        [Description("شماره شبای مشتری")]
        public string CustomerAccountSheba { get; set; }

        [Description("شماره حساب مشتری")]
        public string CustomerAccountNumber { get; set; }

        [Description("نام بانک  مشتری")]
        public string CustomerAccountBankName { get; set; }

    }
}
