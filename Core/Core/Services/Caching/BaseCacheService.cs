using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json.Linq;
using Core.Abstractions.Caching;
using Core.Enums;
using System.Linq;
using AutoMapper;
using Core.Abstractions;

namespace Core.Services.Caching
{
    //[Singleton]
    public abstract class BaseCacheService : IBaseCacheService
    {
        protected readonly IMapper _mapper;
        protected readonly IGuard _guard;

        public ICacheProvider CacheProvider { get; }

        protected readonly string _appName;
        public abstract string CacheName { get; }
        public abstract bool HasSaveBehavior { get; }
        public virtual DateTimeOffset? Expiration { get; }

        protected BaseCacheService(ICacheProvider cacheProvider)
        {
            CacheProvider = cacheProvider;
            
            _mapper = ServiceLocator.GetService<IMapper>();
            _guard = ServiceLocator.GetService<IGuard>();
            _appName = string.IsNullOrWhiteSpace(cacheProvider.AppName) ? "*" : cacheProvider.AppName.ToString();
        }

        protected virtual string CreateUniqueKeyName(CacheKey key)
        {
            return $"{_appName}#{CacheName}#{key.ToString() ?? "?"}";
        }

        protected virtual string GetKeyPattern()
        {
            return $"{_appName}#{CacheName}#";
        }

        protected async Task SetHandler<T>(CacheKey key, T value, DateTimeOffset? expiration)
        {
            await CacheProvider.UpdateOrCreateAsync(CreateUniqueKeyName(key), value, expiration ?? Expiration, _appName);
            await AfterSet(key, value, expiration);
        }

        protected async Task<T> GetHandler<T>(CacheKey key, Func<Task<T>> func, bool useCache, DateTimeOffset? expiration)
        {
            if (HasSaveBehavior)
            {
                if (useCache)
                    return await CacheProvider.GetOrCreateAsync(CreateUniqueKeyName(key), func, expiration ?? Expiration, _appName);

                return await CacheProvider.UpdateOrCreateAsync(CreateUniqueKeyName(key), func, expiration ?? Expiration, _appName);
            }
            else
            {
                if (useCache)
                    return await CacheProvider.GetAsync(CreateUniqueKeyName(key), func, _appName);

                return await func();
            }
        }

        protected virtual Task AfterSet<T>(CacheKey key, T value, DateTimeOffset? expiration)
        {
            return Task.CompletedTask;
        }
    }

    public abstract class BaseGCqService : BaseCacheService, IBaseGCacheService //Generic Mode
    {
        protected BaseGCqService(ICacheProvider cacheProvider) : base(cacheProvider)
        {
        }

        public virtual async Task Set<T>(CacheKey key, T value, DateTimeOffset? expiration = null)
        {
            await SetHandler(key, value, expiration);
        }

        public abstract Task<T> Generate<T>(CacheKey key);

        public virtual async Task<T> Get<T>(CacheKey key, bool useCache = true, DateTimeOffset? expiration = null)
        {
            Task<T> Func()
            {
                return Generate<T>(key);
            }

            return await GetHandler(key, Func, useCache, expiration);
        }
    }

    public abstract class BaseRCqService<T> : BaseCacheService, IBaseRCacheService<T> //ReadOnly
    {
        protected BaseRCqService(ICacheProvider cacheProvider) : base(cacheProvider)
        {
        }

        public virtual async Task<T> Get(CacheKey key)
        {
            var (hasValue, result) = await CacheProvider.GetAsync<T>(CreateUniqueKeyName(key), _appName);
            if (hasValue)
                return result;

            return default!;
        }

        public virtual async Task<(bool, T, TimeSpan?)> GetWithExpiry(CacheKey key)
        {
            return await CacheProvider.GetWithExpiryAsync<T>(CreateUniqueKeyName(key), _appName);
        }
    }


    public abstract class BaseSCqService<T> : BaseCacheService, IBaseSCacheService<T> //string with json serialization mode
    {
        protected BaseSCqService(ICacheProvider cacheProvider) : base(cacheProvider)
        {
        }

        public virtual async Task Set(CacheKey key, T value, DateTimeOffset? expiration = null)
        {
            await SetHandler(key, value, expiration);
        }

        public void SetSync(CacheKey key, T value, DateTimeOffset? expiration = null) //Just for use in sky
        {
            CacheProvider.Add(CreateUniqueKeyName(key), value, expiration ?? Expiration, _appName);
        }

        public abstract Task<T> Generate(CacheKey key);

        public virtual async Task<T> Get(CacheKey key, bool useCache = true, DateTimeOffset? expiration = null)
        {
            Task<T> Func()
            {
                return Generate(key);
            }

            return await GetHandler(key, Func, useCache, expiration);
        }

        public virtual async Task<Dictionary<string, T>> Get(IEnumerable<string> keys, bool useCache = true, DateTimeOffset? expiration = null)
        {
            var mappedKeys = keys.Distinct().ToDictionary(x => CreateUniqueKeyName(x), x => x);

            var result = await CacheProvider.GetAsync<T>(mappedKeys.Select(x => x.Key));

            var finalResult = new Dictionary<string, T>();

            foreach (var mappedKey in mappedKeys)
            {
                if (!result.TryGetValue(mappedKey.Key, out var val))
                {
                    var value = await Get(mappedKey.Value, false);
                    finalResult.Add(mappedKey.Value, value);
                }
                else
                    finalResult.Add(mappedKey.Value, val);
            }

            return finalResult;
        }

        public virtual async Task ClearAll()
        {
            await CacheProvider.SRemoveWithPatternAsync(GetKeyPattern());
        }
    }

    public abstract class BaseHCqService<T> : BaseCacheService, IBaseHCacheService<T> //HashSet
    {
        protected BaseHCqService(ICacheProvider cacheProvider) : base(cacheProvider)
        {
        }

        public virtual async Task Set(CacheKey key, CacheKey field, T value)
        {
            await CacheProvider.HUpdateOrCreateAsync(CreateUniqueKeyName(key), field, value, _appName);
            await AfterSet(key, value, null);
        }

        public abstract Task<T> Generate(CacheKey key, CacheKey field);

        public virtual async Task<T> Get(CacheKey key, CacheKey field, bool useCache = true)
        {
            Task<T> Func()
            {
                return Generate(key, field);
            }

            if (HasSaveBehavior)
            {
                if (useCache)
                    return await CacheProvider.HGetOrCreateAsync(CreateUniqueKeyName(key), field, Func, _appName);

                var value = await CacheProvider.HUpdateOrCreateAsync(CreateUniqueKeyName(key), field, Func, _appName);
                await AfterSet(key, value, null);
                return value;
            }
            else
            {
                if (useCache)
                    return await CacheProvider.HGetAsync(CreateUniqueKeyName(key), field, Func, _appName);

                return await Func();
            }
        }

        public virtual async Task Remove(CacheKey key)
        {
            await CacheProvider.HRemoveAsync<T>(CreateUniqueKeyName(key), _appName);
        }

        public virtual async Task Remove(CacheKey key, CacheKey field)
        {
            await CacheProvider.HRemoveAsync<T>(CreateUniqueKeyName(key), field, _appName);
        }

        public abstract Task<Dictionary<string, T>> GenerateAll(CacheKey key);

        public virtual async Task<Dictionary<string, T>> GetAll(CacheKey key, bool useCache = true)
        {
            Task<Dictionary<string, T>> Func()
            {
                return GenerateAll(key);
            }

            //inja SaveBehavier ro barresi nakardim chon be dalile bussinese ^ dar HashSet ha, GetAll bedone Create nadarim
            if (useCache)
                return await CacheProvider.HGetAllOrCreateAsync(CreateUniqueKeyName(key), Func, _appName);

            var value = await CacheProvider.HUpdateAllOrCreateAsync(CreateUniqueKeyName(key), Func, _appName);
            await AfterSet(key, value, null);
            return value;
        }

        public virtual async Task ClearAll()
        {
            await CacheProvider.HRemoveWithPatternAsync(GetKeyPattern());
        }
    }

    public abstract class BaseSInvokeCqService<T> : BaseCacheService, IBaseSInvokeCacheService<T>
    {
        protected BaseSInvokeCqService(ICacheProvider cacheProvider) : base(cacheProvider)
        {
        }
        public abstract Task<T> Generate(CacheKey key);

        public async Task<T> Get(CacheKey key, bool useCache = true)
        {
            Task<T> Func()
            {
                return Generate(key);
            }

            if (HasSaveBehavior)
            {
                if (useCache)
                    return await CacheProvider.GetOrCreateAsync(CreateUniqueKeyName(key), Func, appName: _appName);

                var value = await CacheProvider.UpdateOrCreateAsync(CreateUniqueKeyName(key), Func, appName: _appName);
                await AfterSet(key, value, null);
                return value;
            }
            else
            {
                if (useCache)
                    return await CacheProvider.GetAsync(CreateUniqueKeyName(key), Func, appName: _appName);

                return await Func();
            }
        }

        public virtual async Task<Dictionary<string, T>> Get(IEnumerable<string> keys, bool useCache = true, DateTimeOffset? expiration = null)
        {
            var mappedKeys = keys.Distinct().ToDictionary(x => CreateUniqueKeyName(x), x => x);

            var result = await CacheProvider.GetAsync<T>(mappedKeys.Select(x => x.Key));

            var finalResult = new Dictionary<string, T>();

            foreach (var mappedKey in mappedKeys)
            {
                if (!result.TryGetValue(mappedKey.Key, out var val))
                {
                    var value = await Get(mappedKey.Value, false);
                    finalResult.Add(mappedKey.Value, value);
                }
                else
                    finalResult.Add(mappedKey.Value, val);
            }

            return finalResult;
        }

        public async Task Set(CacheKey key, T value)
        {
            await CacheProvider.UpdateOrCreateAsync(CreateUniqueKeyName(key), value, appName: _appName);
            await AfterSet(key, value, null);
        }

        public async Task Remove(CacheKey key)
        {
            await CacheProvider.RemoveAsync<T>(CreateUniqueKeyName(key), _appName);
        }

        public virtual async Task ClearAll()
        {
            await CacheProvider.SRemoveWithPatternAsync(GetKeyPattern());
        }
    }

    public abstract class BaseHInvokeCqService<T> : BaseCacheService, IBaseHInvokeCacheService<T> //HashSet
    {
        protected BaseHInvokeCqService(ICacheProvider cacheProvider) : base(cacheProvider)
        {
        }

        public virtual async Task Set(CacheKey key, CacheKey field, T value)
        {
            await CacheProvider.HUpdateOrCreateAsync(CreateUniqueKeyName(key), field, value, _appName);
            await AfterSet(key, value, null);
        }

        public abstract Task<T> Generate(CacheKey key, CacheKey field);

        public virtual async Task<T> Get(CacheKey key, CacheKey field, bool useCache = true)
        {
            Task<T> Func()
            {
                return Generate(key, field);
            }

            if (HasSaveBehavior)
            {
                if (useCache)
                    return await CacheProvider.HGetOrCreateAsync(CreateUniqueKeyName(key), field, Func, _appName);

                var value = await CacheProvider.HUpdateOrCreateAsync(CreateUniqueKeyName(key), field, Func, _appName);
                await AfterSet(key, value, null);
                return value;
            }
            else
            {
                if (useCache)
                    return await CacheProvider.HGetAsync(CreateUniqueKeyName(key), field, Func, _appName);

                return await Func();
            }
        }

        public virtual async Task Remove(CacheKey key)
        {
            await CacheProvider.HRemoveAsync<T>(CreateUniqueKeyName(key), _appName);
        }

        public virtual async Task Remove(CacheKey key, CacheKey field)
        {
            await CacheProvider.HRemoveAsync<T>(CreateUniqueKeyName(key), field, _appName);
        }

        public abstract Task<Dictionary<string, T>> GenerateAll(CacheKey key);

        public virtual async Task<Dictionary<string, T>> GetAll(CacheKey key, bool useCache = true)
        {
            Task<Dictionary<string, T>> Func()
            {
                return GenerateAll(key);
            }

            //inja SaveBehavier ro barresi nakardim chon be dalile bussinese ^ dar HashSet ha, GetAll bedone Create nadarim
            if (useCache)
                return await CacheProvider.HGetAllOrCreateAsync(CreateUniqueKeyName(key), Func, _appName);

            var value = await CacheProvider.HUpdateAllOrCreateAsync(CreateUniqueKeyName(key), Func, _appName);
            await AfterSet(key, value, null);
            return value;
        }

        public virtual async Task ClearAll()
        {
            await CacheProvider.HRemoveWithPatternAsync(GetKeyPattern());
        }
    }

    public abstract class BaseRInvokeCqService<T> : BaseCacheService, IBaseRInvokeCacheService<T>
    {
        protected BaseRInvokeCqService(ICacheProvider cacheProvider) : base(cacheProvider)
        {
        }

        public async Task<bool> Set(CacheKey key, T value)
        {
            return await CacheProvider.AddAsync(CreateUniqueKeyName(key), value, absoluteExpiration: null, appName: _appName);
        }

        public async Task<(bool, T)> Get(CacheKey key)
        {
            return await CacheProvider.GetAsync<T>(CreateUniqueKeyName(key), _appName);
        }

        public virtual async Task<bool> Remove(CacheKey key)
        {
            return await CacheProvider.RemoveAsync<T>(CreateUniqueKeyName(key), _appName);
        }
    }

}
