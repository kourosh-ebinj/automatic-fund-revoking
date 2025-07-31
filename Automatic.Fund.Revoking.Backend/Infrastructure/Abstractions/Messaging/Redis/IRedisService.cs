using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Infrastructure.Abstractions.Messaging.Redis
{
    public interface IRedisService
    {
        Task ConnectAsync();
        IConnectionMultiplexer Connection { get; }
        IDatabase Database { get; }
    }
}
