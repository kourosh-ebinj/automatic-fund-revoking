using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Messaging;
using MassTransit;

namespace Core.Abstractions.Messaging
{
    public interface IBaseConsumer<T>: IConsumer<T> where T : BaseConsumerModel
    {

    }
}
