using System;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace LibMs.API.Cache.Concretes
{
	public class RedisCacheService : ICacheService
	{
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var cachedData = await _cache.GetStringAsync(key);
            if (cachedData != null)
            {
                return JsonSerializer.Deserialize<T>(cachedData);
            }

            return default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null)
        {
            if (value != null)
            {
                var options = new DistributedCacheEntryOptions();
                if (absoluteExpiration.HasValue)
                {
                    options.AbsoluteExpirationRelativeToNow = absoluteExpiration.Value;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
            }
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}

