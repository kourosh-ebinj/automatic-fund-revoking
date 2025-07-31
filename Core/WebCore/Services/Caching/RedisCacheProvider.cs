using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Abstractions.Caching;
using Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StackExchange.Redis;
using CommandFlags = StackExchange.Redis.CommandFlags;

namespace WebCore.Services.Caching
{
    public class RedisCacheProvider : ICacheProvider, IDisposable
    {
        private readonly ApplicationSettingModel _applicationSettings;
        public ConnectionMultiplexer ConnectionMultiplexer { get; set; }
        public IDatabase Redis { get; set; }

        protected byte RedisDb = 0;
        public string Prefix;
        private const string InitChar = "^";
        private string _appname;

        string ICacheProvider.AppName => _appname;

        public RedisCacheProvider(ApplicationSettingModel applicationSettings, IWebHostEnvironment webHostEnvironment)
        {
            _applicationSettings = applicationSettings;

        }

        public void Initialize(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
                throw new ArgumentNullException(nameof(appName));
            _appname = appName;

            var redisConnection = _applicationSettings.Redis.ConnectionString;
            //var match = Regex.Match(redisConnection, @"(db=\w*)");
            redisConnection = Regex.Replace(redisConnection, @"(db=\w*),|(db=\w*)|,(db=\w*)", "");

            ConnectionMultiplexer = ConnectionMultiplexer.Connect(redisConnection);
            Redis = ConnectionMultiplexer.GetDatabase(RedisDb);
        }

        protected virtual string GetCacheKey<T>(string id, string appName = null)
        {
            return id;
        }
        public (bool, T) Get<T>(string key, string appName = null)
        {
            var redisValue = Redis.StringGet(GetCacheKey<T>(key, appName));
            var result = redisValue.HasValue ? JsonConvert.DeserializeObject<T>(redisValue) : default;
            return (redisValue.HasValue, result);
        }

        public async Task<(bool, T)> GetAsync<T>(string key, string appName = null)
        {
            var redisValue = await Redis.StringGetAsync(GetCacheKey<T>(key, appName));
            return (redisValue.HasValue, redisValue.HasValue ? JsonConvert.DeserializeObject<T>(redisValue) : default);
        }

        public Task<Dictionary<string, T>> GetAsync<T>(IEnumerable<string> keys, string appName = null)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool, T, TimeSpan?)> GetWithExpiryAsync<T>(string key, string appName = null)
        {
            var redisValueWithExpiry = await Redis.StringGetWithExpiryAsync(GetCacheKey<T>(key, appName));
            var result = redisValueWithExpiry.Value.HasValue ? JsonConvert.DeserializeObject<T>(redisValueWithExpiry.Value) : default;
            return (redisValueWithExpiry.Value.HasValue, result, redisValueWithExpiry.Expiry);
        }

        public async Task<(bool, T)> GetSetAsync<T>(string key, T value, string appName = null)
        {
            var redisValue = await Redis.StringGetSetAsync(GetCacheKey<T>(key, appName), JsonConvert.SerializeObject(value, CustomJsonSettings.GetJsonSerializerSettings(false)));
            var result = redisValue.HasValue ? JsonConvert.DeserializeObject<T>(redisValue) : default;
            return (redisValue.HasValue, result);
        }

        public async Task<(bool, T)> GetAndDeleteAsync<T>(string key, string appName = null)
        {
            var cacheKey = GetCacheKey<T>(key, appName);
            var tran = Redis.CreateTransaction();
            var getResult = tran.StringGetAsync(cacheKey);
            tran.KeyDeleteAsync(cacheKey);
            await tran.ExecuteAsync();
            var redisValue = await getResult;

            var result = redisValue.HasValue ? JsonConvert.DeserializeObject<T>(redisValue) : default;
            return (redisValue.HasValue, result);
        }

        public async Task<IEnumerable<T>> GetWithPatternAsync<T>(string key, string appName = null)
        {
            var endPoints = ConnectionMultiplexer.GetEndPoints();

            var redisValues = new List<T>();

            foreach (var endPoint in endPoints)
            {
                var server = ConnectionMultiplexer.GetServer(endPoint);
                if (!server.IsReplica && server.IsConnected)
                {
                    var redisKeys = server.KeysAsync(RedisDb, $"*{GetCacheKey<T>(key, appName)}*");

                    var tasks = new List<Task<RedisValue>>();
                    await foreach (var redisKey in redisKeys)
                    {
                        tasks.Add(Redis.StringGetAsync(redisKey));
                    }

                    await Task.WhenAll(tasks);

                    redisValues = tasks.Select(x => x.Result.HasValue ? JsonConvert.DeserializeObject<T>(x.Result) : default).ToList();
                    break;
                }

            }

            return redisValues;
        }

        public async Task<IEnumerable<T>> RemoveWithPatternAsync<T>(string key, string appName = null)
        {
            var endPoints = ConnectionMultiplexer.GetEndPoints();

            var redisValues = new List<T>();

            foreach (var endPoint in endPoints)
            {
                var server = ConnectionMultiplexer.GetServer(endPoint);
                if (!server.IsReplica && server.IsConnected)
                {
                    var redisKeys = server.KeysAsync(RedisDb, $"*{GetCacheKey<T>(key, appName)}*");

                    await foreach (var redisKey in redisKeys)
                    {
                        await Redis.KeyDeleteAsync(redisKey.ToString());
                    }
                    break;
                }

            }

            return redisValues;
        }

        public async Task SRemoveWithPatternAsync(string key, string appName = null)
        {
            var removed = await Redis.KeyDeleteAsync(key);
            if (!removed)
                throw new InvalidOperationException($"{key} key can not be removed.");
        }

        public Task HRemoveWithPatternAsync(string key, string appName = null)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> valueFactory, string appName = null)
        {
            var (hasValue, result) = await GetAsync<T>(key, appName);
            if (hasValue)
                return result;

            return await valueFactory();
        }

        public bool Add<T>(string key, T value, DateTimeOffset? absoluteExpiration = null, string appName = null)
        {
            var expiryTimeSpan = absoluteExpiration?.Subtract(DateTime.Now);
            return Redis.StringSet(GetCacheKey<T>(key, appName), JsonConvert.SerializeObject(value, CustomJsonSettings.GetJsonSerializerSettings(false)), expiryTimeSpan);
        }

        public Task<bool> AddAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration = null, string appName = null)
        {
            return AddAsync(key, value, absoluteExpiration, false, appName);

            //var expiryTimeSpan = absoluteExpiration?.Subtract(DateTime.Now);
            //return await Redis.StringSetAsync(GetCacheKey<T>(key, appName), JsonConvert.SerializeObject(value, CustomJsonSettings.GetJsonSerializerSettings(false)), expiryTimeSpan);
        }

        public async Task<bool> AddAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration = null, bool whenNotExist = false, string appName = null)
        {
            var expiryTimeSpan = absoluteExpiration?.Subtract(DateTime.Now);
            return await Redis.StringSetAsync(GetCacheKey<T>(key, appName), JsonConvert.SerializeObject(value, CustomJsonSettings.GetJsonSerializerSettings(false)), expiryTimeSpan, whenNotExist ? When.NotExists : When.Always);
        }

        public async Task<long> IncrementAsync<T>(string key, long increment = 1, string appName = null)
        {
            return await Redis.StringIncrementAsync(GetCacheKey<T>(key, appName), increment);
        }

        public async Task<TimeSpan?> KeyTimeToLiveAsync<T>(string key, string appName = null)
        {
            return await Redis.KeyTimeToLiveAsync(GetCacheKey<T>(key, appName));
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<T> valueFactory, DateTimeOffset? absoluteExpiration = null, string appName = null)
        {
            var (hasValue, result) = await GetAsync<T>(key, appName);
            if (hasValue)
                return result;

            var value = valueFactory();

            await AddAsync(key, value, absoluteExpiration, appName);

            return value;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> valueFactory, DateTimeOffset? absoluteExpiration = null, string appName = null)
        {
            var (hasValue, result) = await GetAsync<T>(key, appName);
            if (hasValue)
                return result;

            var value = await valueFactory();

            await AddAsync(key, value, absoluteExpiration, appName);

            return value;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration = null, string appName = null)
        {
            var (hasValue, result) = await GetAsync<T>(key, appName);
            if (hasValue)
                return result;

            await AddAsync(key, value, absoluteExpiration, appName);

            return value;
        }

        public T GetOrCreate<T>(string key, Func<T> valueFactory, DateTimeOffset? absoluteExpiration = null, string appName = null)
        {
            var (hasValue, result) = Get<T>(key, appName);
            if (hasValue)
                return result;

            var value = valueFactory();

            Add(key, value, absoluteExpiration, appName);

            return value;
        }

        public async Task<T> UpdateOrCreateAsync<T>(string key, Func<Task<T>> valueFactory, DateTimeOffset? absoluteExpiration = null, string appName = null)
        {
            var value = await valueFactory();

            await AddAsync(key, value, absoluteExpiration, appName);

            return value;
        }

        public async Task<T> UpdateOrCreateAsync<T>(string key, Func<T> valueFactory, DateTimeOffset? absoluteExpiration = null, string appName = null)
        {
            var value = valueFactory();

            await AddAsync(key, value, absoluteExpiration, appName);

            return value;
        }

        public async Task<T> UpdateOrCreateAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration = null, string appName = null)
        {
            await AddAsync(key, value, absoluteExpiration, appName);

            return value;
        }

        public void Remove<T>(string key, string appName = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveAsync<T>(string key, string appName = null)
        {
            return await Redis.KeyDeleteAsync(GetCacheKey<T>(key, appName));
        }

        public async Task SetRangeAsync<T>(string key, T value, long offset, string appName = null)
        {
            await Redis.StringSetRangeAsync(GetCacheKey<T>(key, appName), offset, JsonConvert.SerializeObject(value, CustomJsonSettings.GetJsonSerializerSettings(false)));
        }

        /// <summary>
        ///  hash set value with given key and field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="valueFactory"></param>
        /// <param name="appName"></param>
        /// <returns> True if the given field is new, False if the given field had exist and has been updated. </returns>
        public async Task<bool> HSetAsync<T>(string key, string field, T value, string appName = null)
        {
            return await Redis.HashSetAsync(GetCacheKey<T>(key, appName), field, JsonConvert.SerializeObject(value, CustomJsonSettings.GetJsonSerializerSettings(false)), When.Always);
        }

        public async Task HSetAsync<T>(string key, Dictionary<string, T> values, string appName = null)
        {
            await Redis.HashSetAsync(GetCacheKey<T>(key, appName), values.Select(s => new HashEntry(s.Key, JsonConvert.SerializeObject(s.Value, CustomJsonSettings.GetJsonSerializerSettings(false)))).ToArray());
        }

        public async Task<(bool, Dictionary<string, T>)> HGetAllAsync<T>(string key, string appName = null) //inja InitChar check nemishe va ehtemale invalid bodane data vojod dare
        {
            var values = await Redis.HashGetAllAsync(GetCacheKey<T>(key, appName));

            var result = values.ToDictionary<HashEntry, string, T>(item => item.Name, item => JsonConvert.DeserializeObject<T>(item.Value));

            return (values.Any(), result);
        }

        public async Task<Dictionary<string, T>> HGetAllOrCreateAsync<T>(string key, Func<Task<Dictionary<string, T>>> valueFactory, string appName = null)
        {
            var values = await Redis.HashGetAllAsync(GetCacheKey<T>(key, appName), CommandFlags.None);

            var result = values.ToDictionary<HashEntry, string, T>(item => item.Name, item => JsonConvert.DeserializeObject<T>(item.Value));

            if (!result.ContainsKey(InitChar))
            {
                result = await valueFactory();
                result.Add(InitChar, default);
                await HSetAsync(key, result, appName);
            }

            result.Remove(InitChar);

            return result;
        }

        public async Task<Dictionary<string, T>> HUpdateAllOrCreateAsync<T>(string key, Func<Task<Dictionary<string, T>>> valueFactory, string appName = null)
        {
            await RemoveAsync<T>(key, appName); // baraye inke field haye ghabli remove shan

            var result = await valueFactory();
            result.Add(InitChar, default);

            await HSetAsync(key, result, appName);

            result.Remove(InitChar);

            return result;
        }

        public Task<(bool, T)> SGetAsync<T>(string key, string appName = null)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool, T)> HGetAsync<T>(string key, string field, string appName = null)
        {
            var redisValue = await Redis.HashGetAsync(GetCacheKey<T>(key, appName), field, CommandFlags.None);

            return (redisValue.HasValue, redisValue.HasValue ? JsonConvert.DeserializeObject<T>(redisValue) : default);
        }

        public async Task<T> HGetAsync<T>(string key, string field, Func<Task<T>> valueFactory, string appName = null)
        {
            var (hasValue, value) = await HGetAsync<T>(key, field, appName);
            if (hasValue)
                return value;

            return await valueFactory();
        }

        public async Task<bool> HRemoveAsync<T>(string key, string field, string appName = null)
        {
            return await Redis.HashDeleteAsync(GetCacheKey<T>(key, appName), field);
        }

        public async Task<bool> HRemoveAsync<T>(string key, string appName = null)
        {
            return await Redis.KeyDeleteAsync(GetCacheKey<T>(key, appName));
        }

        public async Task<T> HGetOrCreateAsync<T>(string key, string field, Func<Task<T>> valueFactory, string appName = null)
        {
            var (hasValue, result) = await HGetAsync<T>(key, field, appName);
            if (hasValue)
                return result;

            var value = await valueFactory();
            await HSetAsync(key, field, value, appName);

            return value;
        }

        public async Task<T> HUpdateOrCreateAsync<T>(string key, string field, Func<Task<T>> valueFactory, string appName = null)
        {
            var value = await valueFactory();

            await HSetAsync(key, field, value, appName);

            return value;
        }

        public async Task<T> HUpdateOrCreateAsync<T>(string key, string field, T value, string appName = null)
        {
            await HSetAsync(key, field, value, appName);

            return value;
        }

        public async Task<Dictionary<string, T>> HUpdateOrCreateAsync<T>(string key, Func<Task<Dictionary<string, T>>> valueFactory, string appName = null)
        {
            var value = await valueFactory();

            await HSetAsync(key, value, appName);

            return value;
        }

        public void Dispose()
        {
            ConnectionMultiplexer?.Dispose();
        }

    }

    public static class CustomJsonSettings
    {
        public static JsonSerializerSettings SerializerSettings;
        public static JsonSerializerSettings SerializerSettingsWithOutCustomConverters;

        public static JsonSerializerSettings GetJsonSerializerSettings(bool useCustomConverters = true)
        {
            if (useCustomConverters)
            {
                //if (SerializerSettings == null)
                //{
                //    SerializerSettings = MBCore.Instance.Container.Resolve<IOptions<MvcNewtonsoftJsonOptions>>().Value.SerializerSettings;
                //}

                return SerializerSettings;
            }

            if (SerializerSettingsWithOutCustomConverters == null)
            {
                SerializerSettingsWithOutCustomConverters = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Converters = new List<JsonConverter>
                    {
                        new StringEnumConverter
                        {
                            CamelCaseText = false,
                            AllowIntegerValues = true
                        }
                    },
                    DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
                };
            }

            return SerializerSettingsWithOutCustomConverters;
        }
    }
}
