using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Models.Responses
{
    public record UserFundRs
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int FundId { get; set; }

    }
}
