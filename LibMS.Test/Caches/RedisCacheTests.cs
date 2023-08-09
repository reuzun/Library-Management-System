using System;
using System.Text;
using System.Text.Json;
using LibMS.API.Cache.Concretes;
using LibMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;

namespace LibMS.Test
{
    public class RedisCacheTests
    {
        // Mock<IDistributedCache> _cache;
        RedisCacheService _cacheService;

        public RedisCacheTests()
        {
            _cacheService = new RedisCacheService(
                    new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()))
                );
        }

        [Fact]
        public async Task CacheTest_Write_Read()
        {
            var key = "Key_01";
            var valToSave = "Hello";

            await _cacheService.SetAsync(key, valToSave);
            await _cacheService.SetAsync("Key_02", valToSave, TimeSpan.FromMilliseconds(500));

            await _cacheService.RemoveAsync("Key_02");

            var savedVal = await _cacheService.GetAsync<string>(key);
            Assert.NotNull(savedVal);
            Assert.Equivalent(savedVal, valToSave);
            Assert.Null(await _cacheService.GetAsync<string>("Key_02"));
        }


    }
}

