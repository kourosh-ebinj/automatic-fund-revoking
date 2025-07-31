using System;
using System.Collections.Generic;

namespace Application.Models.Responses.ThirdParties.Rayan
{
    public record RayanCustomerInfoRs
    {
        public long CustomerId { get; set; }
        public byte IsLegal { get; set; }
        public byte? IsStockCreditPurchase { get; set; }
        public long CustomerStatusId { get; set; }
        public byte isIranian { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string BirthCertNumber { get; set; }
        public string NationalId { get; set; }
        public string NationalCode { get; set; }
        public string Phone { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string ReferredBy { get; set; }
        public string SiteUsername { get; set; }
        public string RegistrationDate { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string DlNumber { get; set; }
        public byte IsProfitIssue { get; set; }
        public int ProfitRate { get; set; }
        public byte IsPortfo { get; set; }
        public int? SejamStatusTypeId { get; set; }
        public string BirthCertificationId { get; set; }
        public string FullName { get; set; }

        public IEnumerable<RayanBankAccountRs> BankAccounts { get; set; }
    }

    public class RayanBankAccountRs
    {
        public long BankAccountId { get; set; }
        public string AccountNumber { get; set; }
        public string BaTypeName { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string BankBranchCode { get; set; }
        public string shabaNumber { get; set; }
        public byte IsDefault { get; set; }

    }

}
