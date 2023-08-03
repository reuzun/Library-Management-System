using System;
using System.Text.Json;
using LibMs.API.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace LibMs.API.Attributes
{
	public class InvalidateCacheAttribute : ActionFilterAttribute
    {
        private ICacheService? _cache;
        private string _keyToDel;

        public InvalidateCacheAttribute(string key)
		{
            _keyToDel = key;
		}

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _cache = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            _cache.RemoveAsync(_keyToDel);
            Log.Information($"Cache: key {_keyToDel} is invalidated!");
            return Task.CompletedTask;
        }
    }
}

