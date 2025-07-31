using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Messaging.MessageDispatcher
{
    public record RecipientRq
    {
        public long UserId { get; set; }
        public string Destination { get; set; }
    }
}
