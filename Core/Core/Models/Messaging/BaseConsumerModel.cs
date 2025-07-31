using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Messaging
{
    public record BaseConsumerModel
    {
        public string SenderAppName { get; set; }
    }
}
