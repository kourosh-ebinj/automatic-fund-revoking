using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Requests.Messaging;
using Core.Abstractions.Messaging;
using MassTransit;

namespace Application.Services.Messaging.Consumers
{
    public interface IReverseOrderToDraftConsumer : IBaseConsumer<ReverseOrderToDraftRq>
    {
    }
}
