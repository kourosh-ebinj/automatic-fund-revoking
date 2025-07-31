using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Models.Responses
{
    public record BankRs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public int BackOfficeBankId { get; set; }
        //public string ProviderClassName { get; set; }
        public bool IsEnabled { get; set; }

    }
}
