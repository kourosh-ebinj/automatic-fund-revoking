using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Models.Messaging.MessageDispatcher;

namespace WebCore.Services
{
    public class EmailService : MessageDispacherService, IEmailService, IDisposable
    {
        private bool disposedValue;

        public async Task Send(EmailRq request)
        {
            var items = new List<EmailRq>
            {
                request
            };

            await Send(items);
        }

        public async Task Send(IEnumerable<EmailRq> request)
        {
            var messages = new List<SendMessageRq>();

            foreach (var message in request)
            {
                if (!message.Recipients.Any())
                    throw new ArgumentException(nameof(message.Recipients));

                var msg = new SendMessageRq()
                {
                    Body = message.Body,
                    Metadata = message.Metadata,
                    SendDate = message.SendDate,
                    SenderAppName = message.SenderAppName,
                    SenderId = message.UserId,
                    Type = Core.Enums.Messaging.MessageDispatcher.MessageType.Email,
                    Title = message.Title,
                    ProviderId = message.ProviderId,
                };
                msg.Recipients = message.Recipients;

                messages.Add(msg);
            }

            await base.SendMessage(messages);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~EmailService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
