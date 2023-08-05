using System;
using LibMs.API.Cache;
using LibMs.API.Cache.Concretes;

namespace LibMs.API
{
    public class CacheServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CacheServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICacheService CreateCacheService(string provider)
        {
            switch (provider)
            {
                case "MemoryCache":
                    return (ICacheService)_serviceProvider.GetService<MemoryCacheService>();
                case "RedisCache":
                    return (ICacheService)_serviceProvider.GetService<RedisCacheService>();
                default:
                    throw new ArgumentException("Invalid cache provider");
            }
        }
    }
}

