using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Abstractions.Caching
{
    public interface ICacheProvider
    {
        string AppName { get; }

        //string GetCacheKey<T>(string id);
        void Initialize(string appName);
        (bool, T) Get<T>(string key, string appName = null);
        Task<(bool, T)> GetAsync<T>(string key, string appName = null);
        Task<Dictionary<string, T>> GetAsync<T>(IEnumerable<string> keys, string appName = null);
        Task<(bool, T, TimeSpan?)> GetWithExpiryAsync<T>(string key, string appName = null);
        Task<(bool, T)> GetSetAsync<T>(string key, T value, string appName = null);
        Task<(bool, T)> GetAndDeleteAsync<T>(string key, string appName = null);
        Task<IEnumerable<T>> GetWithPatternAsync<T>(string key, string appName = null);
        Task<IEnumerable<T>> RemoveWithPatternAsync<T>(string key, string appName = null);
        Task SRemoveWithPatternAsync(string key, string appName = null);
        Task HRemoveWithPatternAsync(string key, string appName = null);
        Task<T> GetAsync<T>(string key, Func<Task<T>> valueFactory, string appName = null);
        bool Add<T>(string key, T value, DateTimeOffset? absoluteExpiration = null, string appName = null);
        Task<bool> AddAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration = null, string appName = null);
        Task<bool> AddAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration = null, bool whenNotExist = false, string appName = null);
        Task<long> IncrementAsync<T>(string key, long increment = 1, string appName = null);
        Task<TimeSpan?> KeyTimeToLiveAsync<T>(string key, string appName = null);
        Task<T> GetOrCreateAsync<T>(string key, Func<T> valueFactory, DateTimeOffset? absoluteExpiration = null, string appName = null);
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> valueFactory, DateTimeOffset? absoluteExpiration = null, string appName = null);
        Task<T> GetOrCreateAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration = null, string appName = null);
        T GetOrCreate<T>(string key, Func<T> valueFactory, DateTimeOffset? absoluteExpiration = null, string appName = null);
        Task<T> UpdateOrCreateAsync<T>(string key, Func<Task<T>> valueFactory, DateTimeOffset? absoluteExpiration = null, string appName = null);
        Task<T> UpdateOrCreateAsync<T>(string key, Func<T> valueFactory, DateTimeOffset? absoluteExpiration = null, string appName = null);
        Task<T> UpdateOrCreateAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration = null, string appName = null);
        void Remove<T>(string key, string appName = null);
        Task<bool> RemoveAsync<T>(string key, string appName = null);
        Task SetRangeAsync<T>(string key, T value, long offset, string appName = null);
        Task<bool> HSetAsync<T>(string key, string field, T value, string appName = null);
        Task HSetAsync<T>(string key, Dictionary<string, T> values, string appName = null);
        Task<(bool, Dictionary<string, T>)> HGetAllAsync<T>(string key, string appName = null);
        Task<Dictionary<string, T>> HGetAllOrCreateAsync<T>(string key, Func<Task<Dictionary<string, T>>> valueFactory, string appName = null);
        Task<Dictionary<string, T>> HUpdateAllOrCreateAsync<T>(string key, Func<Task<Dictionary<string, T>>> valueFactory, string appName = null);
        Task<(bool, T)> HGetAsync<T>(string key, string field, string appName = null);
        Task<T> HGetAsync<T>(string key, string field, Func<Task<T>> valueFactory, string appName = null);
        Task<bool> HRemoveAsync<T>(string key, string field, string appName = null);
        Task<bool> HRemoveAsync<T>(string key, string appName = null);
        Task<T> HGetOrCreateAsync<T>(string key, string field, Func<Task<T>> valueFactory, string appName = null);
        Task<T> HUpdateOrCreateAsync<T>(string key, string field, Func<Task<T>> valueFactory, string appName = null);
        Task<T> HUpdateOrCreateAsync<T>(string key, string field, T value, string appName = null);
        Task<Dictionary<string, T>> HUpdateOrCreateAsync<T>(string key, Func<Task<Dictionary<string, T>>> valueFactory, string appName = null);
    }
}
