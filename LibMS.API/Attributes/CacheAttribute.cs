using System;
using System.Text;
using System.Text.Json;
using LibMS.API.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace LibMS.API.Attributes
{
    public class CacheAttribute : ActionFilterAttribute
    {
        public int Duration { get; set; }

        public CacheAttribute()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Access the ICacheService directly from the current scope
            ICacheService _cache = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            // Check if the result is already cached
            var cacheKey = GenerateCacheKey(context);
            var cachedData = await _cache.GetAsync<string>(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                Log.Information($"Cache: {cacheKey} data is found! Returning it.");
                context.Result = new ContentResult
                {
                    Content = cachedData,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
            }
            else
            {
                // Continue with the action execution
                var executedContext = await next();

                if (executedContext.Result is OkObjectResult okObjectResult)
                {
                    var serializedData = JsonSerializer.Serialize(okObjectResult.Value);
                    if(Duration == -1)
                    {
                        Log.Warning("Cache: Setting Keys without Duration can cause Memory and Disk Problems. Since it has the chance to be never deleted and serialized on the Disk.");
                        await _cache.SetAsync(cacheKey, serializedData, null);
                    }
                    else
                    {
                        await _cache.SetAsync(cacheKey, serializedData, TimeSpan.FromSeconds(Duration));
                    }
                    Log.Information($"Cache: {cacheKey} data is saved!");
                }
            }
        }

        private string GenerateCacheKey(FilterContext context)
        {
            var request = context.HttpContext.Request;
            var cacheKey = new StringBuilder();

            cacheKey.Append(request.Path);

            return cacheKey.ToString();
        }
    }
}
