using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;

namespace Application.Models.Responses
{
    public record LimitationRq
    {
        public IEnumerable<LimitationComponentRs> Components { get; set; }
    }

    public record LimitationRs
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public int FundId { get; set; }

        public LimitationTypeEnum LimitationTypeId { get; set; }

    }

    public record LimitationComponentRs
    {
        public int Id { get; set; }
        public LimitationComponentTypeEnum LimitationComponentTypeId { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Error { get; set; }
        public bool Enabled { get; set; }

    }
}
