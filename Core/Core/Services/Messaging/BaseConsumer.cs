using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Abstractions;
using Core.Abstractions.Messaging;
using Core.Models.Messaging;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Core.Services.Messaging
{
    public class BaseConsumer<T> : IBaseConsumer<T> where T : BaseConsumerModel
    {
        protected readonly IMapper _mapper;
        protected readonly IGuard _guard;
        protected readonly ILogger _logger;

        public BaseConsumer()
        {
            _logger = ServiceLocator.GetService<ILogger<T>>();
            _guard = ServiceLocator.GetService<IGuard>();
            _mapper = ServiceLocator.GetService<IMapper>();

        }

        public async virtual Task Consume(ConsumeContext<T> context)
        {
            await Console.Out.WriteLineAsync($"Message (CorrelationId: {context.CorrelationId}, SenderAppName: {context.Message.SenderAppName}) consumed. ");

        }
    }

}
