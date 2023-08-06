using System;
namespace LibMS.API.Cache
{
	public interface ICacheService
	{
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null);
        Task RemoveAsync(string key);
    }
}

