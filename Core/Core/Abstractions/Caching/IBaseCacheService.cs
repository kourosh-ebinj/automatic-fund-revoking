using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Abstractions.Caching
{
    public interface IBaseCacheService
    {

    }

    public interface IBaseGCacheService : IBaseCacheService
    {
        Task Set<T>(CacheKey key, T value, DateTimeOffset? expiration = null);
        Task<T> Get<T>(CacheKey key, bool useCache = true, DateTimeOffset? expiration = null);
    }

    public interface IBaseSCacheService<T> : IBaseCacheService
    {
        Task Set(CacheKey key, T value, DateTimeOffset? expiration = null);
        //void SetSync(CacheKey key, T value, DateTimeOffset? expiration = null); //Just for use in sky
        Task<T> Get(CacheKey key, bool useCache = true, DateTimeOffset? expiration = null);
        Task<Dictionary<string, T>> Get(IEnumerable<string> keys, bool useCache = true, DateTimeOffset? expiration = null);
        Task ClearAll();
    }

    public interface IBaseSStreamerCacheService<T> : IBaseCacheService
    {
        Task Publish(CacheKey key, T value);
        Task Remove(CacheKey key);
    }

    public interface IBaseHStreamerCacheService<T> : IBaseCacheService
    {
        Task Publish(CacheKey key, CacheKey field, T value);
        Task Remove(CacheKey key);
    }

    public interface IBaseRCacheService<T> : IBaseCacheService
    {
        Task<T> Get(CacheKey key);
        Task<(bool, T, TimeSpan?)> GetWithExpiry(CacheKey key);
    }

    public interface IBaseHCacheService<T> : IBaseCacheService
    {
        Task Set(CacheKey key, CacheKey field, T value);
        Task<T> Get(CacheKey key, CacheKey field, bool useCache = true);
        Task Remove(CacheKey key);
        Task Remove(CacheKey key, CacheKey field);
        Task<Dictionary<string, T>> GetAll(CacheKey key, bool useCache = true);
        Task ClearAll();
    }

    public interface IBaseHInvokeCacheService<T> : IBaseCacheService
    {
        Task Set(CacheKey key, CacheKey field, T value);
        Task<T> Get(CacheKey key, CacheKey field, bool useCache = true);
        Task Remove(CacheKey key, CacheKey field);
        Task<Dictionary<string, T>> GetAll(CacheKey key, bool useCache = true);
        Task ClearAll();
    }
    public interface IBaseSInvokeCacheService<T> : IBaseCacheService
    {
        Task Set(CacheKey key, T value);
        Task<T> Get(CacheKey key, bool useCache = true);
        Task<Dictionary<string, T>> Get(IEnumerable<string> keys, bool useCache = true, DateTimeOffset? expiration = null);
        Task Remove(CacheKey key);
        Task ClearAll();
    }

    public interface IBaseRInvokeCacheService<T> : IBaseCacheService
    {
        Task<bool> Set(CacheKey key, T value);
        Task<(bool, T)> Get(CacheKey key);
        Task<bool> Remove(CacheKey key);
    }
}
