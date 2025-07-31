using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services
{
    public class InMemoryCacheService : ICachingService
    {

        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                await Task.Run(() => _memoryCache.Remove(key));
                return true;

            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SetAsync<T>(string key, T item)
        {
            try
            {
                _memoryCache.Set(key, item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SetAsync<T>(string key, T item, TimeSpan ttl)
        {
            try
            {
                _memoryCache.Set(key, item, DateTimeOffset.UtcNow.AddMinutes(ttl.TotalMinutes));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<bool> TryGet<T>(string key, out T value)
        {
            return Task.FromResult( _memoryCache.TryGetValue(key, out value));
        }
    }
}
