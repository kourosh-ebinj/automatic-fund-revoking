using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Models.Responses.ThirdParties.Rayan
{
    public record RayanFundOrderRs
    {
        /// <summary>
        /// شناسه درخواست
        /// </summary>
        public long FundOrderId { get; set; }

        /// <summary>
        /// مبلغ هر واحد
        /// </summary>
        public long? UnitPrice { get; set; }

        public long? OrderAmount { get; set; }

        /// <summary>
        /// شماره حساب سرمایه گذار
        /// </summary>
        public string customerAccountNumber { get; set; }

        public long CustomerId { get; set; }
        public string CustomerName { get; set; }

        /// <summary>
        /// کد ملی سرمایه گذار
        /// </summary>
        public string NationalCode { get; set; }

        /// <summary>
        /// وضعیت درخواست
        /// </summary>
        public string FoStatusName { get; set; }

        public int foStatusId { get; set; }

        /// <summary>
        /// شماره درخواست
        /// </summary>
        public string FundOrderNumber { get; set; }

        /// <summary>
        /// تعداد واحد
        /// </summary>
        public int FundUnit { get; set; }

        public string OrderDate { get; set; }

        public string CreationDate { get; set; }

        public string CreationTime { get; set; }

        public string ModificationDate { get; set; }

        public string ModificationTime { get; set; }

        /// <summary>
        /// تاریخ گواهی
        /// </summary>
        public string LicenseDate { get; set; }

        /// <summary>
        /// مبلغ تضمین
        /// </summary>
        public long GuaranteeAmount { get; set; }

        public long? VatAmount { get; set; }

        public string BranchName { get; set; }

        /// <summary>
        /// نام برنامه ای که ثبت درخواست کرده است
        /// </summary>
        public string AppuserName { get; set; }
        /// <summary>
        /// آی دی که ثبت درخواست کرده است
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///  نوع درخواست
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// نوع سرمایه گذاری
        /// </summary>
        public string FundIssueTypeName { get; set; }

        /// <summary>
        /// پرداخت از طریق
        /// </summary>
        public string OrderPaymentTypeName { get; set; }

        /// <summary>
        /// محل تامین اعتبار
        /// </summary>
        public string FundIssueOriginName { get; set; }

        /// <summary>
        /// کارمزد ثابت
        /// </summary>
        public long FixWage { get; set; }

        /// <summary>
        /// کارمزد متغیر
        /// </summary>
        public long VariableWage { get; set; }

        public string ReceiptNumber { get; set; }

        public string BranchCode { get; set; }

        /// <summary>
        /// شماره تفصیل سرمایه گذار
        /// </summary>
        public string DlNumber { get; set; }

        /// <summary>
        /// شماره گواهی
        /// </summary>
        public long? LicenseNumber { get; set; }

        public string ReceiptComment { get; set; }

        /// <summary>
        /// شماره سند
        /// </summary>
        public string VoucherNumber { get; set; }

        /// <summary>
        /// شماره موقت سند
        /// </summary>
        public string VoucherTempNumber { get; set; }

        public string ToBankAccountId { get; set; }

        public string ReceiptDate { get; set; }

        /// <summary>
        /// نوع سرمایه گذاری
        /// </summary>
        public int FundIssueTypeId { get; set; }

        public string RepresentativeCode { get; set; }

        /// <summary>
        /// قسمتی از سود سرمایه
        /// </summary>
        public decimal? FundIssuePartPercent { get; set; }
        public int FundId { get; set; }
    }

}
