using System;
using LibMS.API;
using LibMS.API.Cache;
using LibMS.API.Cache.Concretes;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace LibMS.Test
{
    public class MemoryCacheTests
    {

        public MemoryCacheTests()
        {
        }

        [Fact]
        public async Task MemoryCache_Save_Read_Successfully()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var memoryCache = new MemoryCacheService(cache);


            await memoryCache.SetAsync<string>("Key_1", "Hello");
            await memoryCache.SetAsync<string>("Key_2", "Hello", TimeSpan.FromMilliseconds(5000));
            await memoryCache.RemoveAsync("Key_2");

            Assert.IsType<MemoryCacheService>(memoryCache);
            Assert.NotNull(await memoryCache.GetAsync<string>("Key_1"));
            Assert.Null(await memoryCache.GetAsync<string>("Key_2"));
        }
    }
}

