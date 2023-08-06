using System;
using LibMS.API.Cache;
using LibMS.API.Cache.Concretes;

namespace LibMS.API
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

