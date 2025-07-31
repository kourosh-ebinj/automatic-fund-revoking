using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Models.Requests
{
    public record BankAccountUpdateRq
    {
        public int Id{ get; set; }
        public string AccountNumber { get; set; }
        public string ShebaNumber { get; set; }
        public int BankId { get; set; }
        public bool IsEnabled { get; set; }

    }

}
