using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums.Messaging.MessageDispatcher;

namespace Core.Models.Messaging.MessageDispatcher
{
    public record SendMessageRq : BaseConsumerModel
    {
        [Required]
        public string Title { get; init; } = default!;

        [Required]
        public string Body { get; init; } = default!;
        public MessageType Type { get; init; }

        [Required]
        public int SenderId { get; init; }
        public DateTime SendDate { get; set; }
        public int? ProviderId { get; init; }

        public string Metadata { get; set; }
        public ICollection<RecipientRq> Recipients { get; set; } = new List<RecipientRq>();
    }
}
