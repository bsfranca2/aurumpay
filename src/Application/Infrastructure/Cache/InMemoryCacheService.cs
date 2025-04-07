using Microsoft.Extensions.Caching.Memory;

namespace AurumPay.Application.Infrastructure.Cache;

public class InMemoryCacheService(IMemoryCache cache) : ICacheService
{
    public Task<T?> GetAsync<T>(string key)
    {
        return Task.FromResult(cache.Get<T>(key));
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var options = new MemoryCacheEntryOptions();
        if (expiry.HasValue) options.SetSlidingExpiration(expiry.Value);
        
        cache.Set(key, value, options);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        cache.Remove(key);
        return Task.CompletedTask;
    }
}