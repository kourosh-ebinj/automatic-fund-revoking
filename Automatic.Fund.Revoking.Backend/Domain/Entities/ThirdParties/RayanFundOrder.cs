using System;
using Domain.Abstractions;

namespace Domain.Entities.ThirdParties
{
    public class RayanFundOrder : EntityBase, ITrackable
    {
        public long Id { get; set; }
        public long FundOrderId { get; set; }
        public long? UnitPrice { get; set; }
        public long? OrderAmount { get; set; }
        public string customerAccountNumber { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string NationalCode { get; set; }
        public string FoStatusName { get; set; }
        public string FundOrderNumber { get; set; }
        public int FundUnit { get; set; }
        public string OrderDate { get; set; }
        public string CreationDate { get; set; }
        public string CreationTime { get; set; }
        public string ModificationDate { get; set; }
        public string ModificationTime { get; set; }
        public string LicenseDate { get; set; }
        public long GuaranteeAmount { get; set; }
        public long? VatAmount { get; set; }
        public string BranchName { get; set; }
        public string AppuserName { get; set; }
        public string UserName { get; set; }
        public string OrderType { get; set; }
        public string FundIssueTypeName { get; set; }
        public string OrderPaymentTypeName { get; set; }
        public string FundIssueOriginName { get; set; }
        public long FixWage { get; set; }
        public long VariableWage { get; set; }
        public string ReceiptNumber { get; set; }
        public string BranchCode { get; set; }
        public string DlNumber { get; set; }
        public long LicenseNumber { get; set; }
        public string ReceiptComment { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherTempNumber { get; set; }
        public int FundId { get; set; }

        public virtual Fund Fund { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedById { get; set; }
    }
}
