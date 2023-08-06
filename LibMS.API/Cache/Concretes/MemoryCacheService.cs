using System;
using Microsoft.Extensions.Caching.Memory;

namespace LibMS.API.Cache.Concretes
{
	public class MemoryCacheService : ICacheService
	{
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            return await Task.FromResult(_cache.Get<T>(key));
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null)
        {
            if (value != null)
            {
                var options = new MemoryCacheEntryOptions();
                if (absoluteExpiration.HasValue)
                {
                    options.AbsoluteExpirationRelativeToNow = absoluteExpiration.Value;
                }

                await Task.FromResult(_cache.Set(key, value, options));
            }
        }

        public async Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            await Task.CompletedTask;
        }
    }
}

