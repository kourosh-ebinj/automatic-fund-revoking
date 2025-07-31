using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Enums;

namespace Application.Models.Responses
{
    public record LimitationComponentValidatorResultRs
    {
        public string StatusMessage { get; set; }
        public LimitationComponentValidatorResultEnum ValidatorResultStatus { get; set; }
    }
}
