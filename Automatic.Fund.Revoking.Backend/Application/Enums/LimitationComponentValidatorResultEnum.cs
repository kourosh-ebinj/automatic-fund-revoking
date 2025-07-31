using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Enums
{
    public enum LimitationComponentValidatorResultEnum : byte
    {
        Rejected = 0,
        NeedsApproval = 1,
        Accepted = 2,
    }
}
