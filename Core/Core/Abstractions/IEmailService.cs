using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models.Messaging.MessageDispatcher;

namespace Core.Abstractions
{
    public interface IEmailService
    {
        Task Send(EmailRq request);
        Task Send(IEnumerable<EmailRq> request);
    }
}
