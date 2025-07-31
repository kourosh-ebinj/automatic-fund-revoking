using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Models.Responses
{
    public record FundRs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DsCode { get; set; }
        public bool IsEnabled { get; set; }

    }
}
