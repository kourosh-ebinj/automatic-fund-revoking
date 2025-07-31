using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums.Messaging.MessageDispatcher;

namespace Core.Models.Messaging.MessageDispatcher
{
    public record MessageRq()
    {
        public string Body { get; set; }
        public int UserId { get; set; }
        public DateTime SendDate { get; set; }
        public string Metadata { get; set; }
        public ICollection<RecipientRq> Recipients { get; set; }
        public string SenderAppName { get; set; }
        public int? ProviderId { get; set; }

    }
}
