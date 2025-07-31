using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Models.Responses.ThirdParties.Rayan
{
    public record RayanCustomerRs
    {
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

    }

}
