﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Responses
{
    public record BankAccountLimitationInputRs
    {
        public long MinBalance { get; set; }
    }
}
