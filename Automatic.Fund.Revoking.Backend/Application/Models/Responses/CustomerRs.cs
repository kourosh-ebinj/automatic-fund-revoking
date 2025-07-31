using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Models.Responses
{
    public record CustomerRs
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string TradingCode { get; set; }
        public string MobileNumber { get; set; }
        public int FundId { get; set; }
        public string FundName { get; set; }

    }
}
