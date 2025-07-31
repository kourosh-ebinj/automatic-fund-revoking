using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums.Messaging.MessageDispatcher;

namespace Core.Models.Messaging.MessageDispatcher
{
    public record EmailRq : MessageRq
    {
        public string Title { get; set; }

    }
}
