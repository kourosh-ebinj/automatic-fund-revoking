using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models.Messaging.MessageDispatcher;
using Core;
using MassTransit;

namespace WebCore.Services
{
    public abstract class MessageDispacherService
    {
        private readonly ISendEndpointProvider _sendEndpoint;

        protected MessageDispacherService()
        {
            _sendEndpoint = ServiceLocator.GetService<ISendEndpointProvider>();
        }

        public async Task SendMessage(IEnumerable<SendMessageRq> messages)
        {
            var sendEndpoint = await _sendEndpoint.GetSendEndpoint(new Uri("queue:sendmessage_queue"));
            await sendEndpoint.SendBatch(messages);
        }

    }
}
