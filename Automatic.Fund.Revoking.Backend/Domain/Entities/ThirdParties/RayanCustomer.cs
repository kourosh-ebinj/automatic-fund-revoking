using System;
using Domain.Abstractions;

namespace Domain.Entities.ThirdParties
{
    public class RayanCustomer : EntityBase, ITrackable
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string BranchName { get; set; }
        public string Personality { get; set; }
        public string CustomerFullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Representative { get; set; }
        public string Nationality { get; set; }
        public string BirthDate { get; set; }
        public string BirthCertificationNumber { get; set; }
        public string NationalIdentifier { get; set; }
        public string CustomerStatusName { get; set; }
        public string AccountNumber { get; set; }
        public string CellPhone { get; set; }
        public string CreationDate { get; set; }
        public string BourseCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedById { get; set; }
        public int FundId { get; set; }

        public virtual Fund Fund { get; set; }

    }
}
