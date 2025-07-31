using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Models.Responses
{
    public record AllLimitationsRs
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int FundId { get; set; }
        public LimitationTypeEnum LimitationTypeId { get; set; }

    }
}
