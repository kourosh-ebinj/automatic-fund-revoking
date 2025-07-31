using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Abstractions.Messaging.Redis;
using Microsoft.Extensions.Logging;
using Core.Abstractions.Messaging;

namespace Infrastructure.Messaging
{
    public class RedisBus : IMessageBus
    {
        private readonly IRedisService _redis;
        private readonly string _hostName;
        private readonly string _channelPrefix;
        private readonly string _messagePrefix;
        private readonly ILogger _logger;

        public RedisBus(IRedisService redis, ILogger<RedisBus> logger)
        {
            _redis = redis;
            //_hostName = Dns.GetHostName() + ':' + Process.GetCurrentProcess().Id;
            //_channelPrefix = shellSettings.Name + ':';
            _messagePrefix = _hostName + '/';
            _logger = logger;
        }

        public async Task SubscribeAsync(string channel, Action<string, string> handler)
        {
            if (_redis.Connection == null)
            {
                await _redis.ConnectAsync();
            }

            try
            {
                var subscriber = _redis.Connection.GetSubscriber();

                await subscriber.SubscribeAsync(_channelPrefix + channel, (redisChannel, redisValue) =>
                {
                    var tokens = redisValue.ToString().Split('/').ToArray();

                    if (tokens.Length != 2 || tokens[0].Length == 0 || tokens[0].Equals(_hostName, StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    handler(channel, tokens[1]);
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "'Unable to subscribe to the channel {ChannelName}'.", _channelPrefix + channel);
            }
        }

        public async Task PublishAsync(string channel, string message)
        {
            if (_redis.Connection == null)
            {
                await _redis.ConnectAsync();
            }

            try
            {
                await _redis.Connection.GetSubscriber().PublishAsync(_channelPrefix + channel, _messagePrefix + message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "'Unable to publish to the channel {ChannelName}'.", _channelPrefix + channel);
            }
        }
    }

}
