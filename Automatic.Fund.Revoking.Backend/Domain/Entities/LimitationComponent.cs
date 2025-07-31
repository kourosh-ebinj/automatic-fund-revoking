using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities
{
    public partial class LimitationComponent: EntityBase, ITrackable
    {
        public int Id { get; set; }
        public int LimitationId { get; set; }
        public LimitationComponentTypeEnum LimitationComponentTypeId { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        //public bool Negate { get; set; }
        public string Error { get; set; }
        public bool Enabled { get; set; }

        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedById { get; set; }

        public Limitation Limitation { get; set; }
    }
}
