using System;
using LibMS.API;
using LibMS.API.Cache;
using LibMS.API.Cache.Concretes;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;

namespace LibMS.Test.Caches
{
    public class CacheFactoryTest
    {
        Mock<IServiceProvider> _serviceProvider;
        CacheServiceFactory _factory;

        public CacheFactoryTest()
        {
            _serviceProvider = new Mock<IServiceProvider>();
            _factory = new CacheServiceFactory(_serviceProvider.Object);
        }

        [Fact]
        public void Create_MemoryCache()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var memoryCache = new MemoryCacheService(cache);
            _serviceProvider
                .Setup(provider => provider.GetService(typeof(MemoryCacheService)))
                .Returns(memoryCache);

            var memoryCacheInstance = _factory.CreateCacheService("MemoryCache");

            Assert.IsType<MemoryCacheService>(memoryCacheInstance);
        }

        [Fact]
        public void Create_RedisCache()
        {
            var cacheService = new RedisCacheService(
                    new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()))
                );

            _serviceProvider
                .Setup(provider => provider.GetService(typeof(RedisCacheService)))
                .Returns(cacheService);

            var memoryCacheInstance = _factory.CreateCacheService("RedisCache");

            Assert.IsType<RedisCacheService>(memoryCacheInstance);
        }


        [Fact]
        public void Create_UnsupportedCache()
        {
            Action act = () => _factory.CreateCacheService("Unsupported!");

            var exception = Assert.Throws<ArgumentException>(act);
        }
    }
}

