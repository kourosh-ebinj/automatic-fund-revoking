using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Models.Requests
{
    public record BankAccountCreateRq
    {
        [Required]
        public string AccountNumber { get; set; }
        
        [Required]
        public string ShebaNumber { get; set; }
        
        [Required]
        public int BankId { get; set; }
        
    }

}
