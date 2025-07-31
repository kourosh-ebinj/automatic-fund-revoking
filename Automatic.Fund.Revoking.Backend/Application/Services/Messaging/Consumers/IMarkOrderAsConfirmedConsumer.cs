using System;
using Application.Models.Requests.Messaging;
using Core.Abstractions.Messaging;

namespace Application.Services.Messaging.Consumers
{
    public interface IMarkOrderAsConfirmedConsumer : IBaseConsumer<MarkOrderAsConfirmedRq>
    {

    }
}
