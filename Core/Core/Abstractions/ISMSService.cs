using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Messaging.MessageDispatcher;

namespace Core.Abstractions
{
    public interface ISMSService
    {
        Task Send(SMSRq request);
        Task Send(IEnumerable<SMSRq> request);
    }
}
