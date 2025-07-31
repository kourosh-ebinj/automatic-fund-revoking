using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities
{
    public class Limitation : EntityBase, ITrackable
    {
        public Limitation()
        {
            LimitationComponents = new List<LimitationComponent>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public LimitationTypeEnum LimitationTypeId { get; set; }
        public int FundId { get; set; }

        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedById { get; set; }

        public Fund Fund { get; set; }
        public virtual ICollection<LimitationComponent> LimitationComponents { get; set; }
    }
}
