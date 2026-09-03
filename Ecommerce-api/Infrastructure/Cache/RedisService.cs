using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Ecommerce_api.Infrastructure.Cache;

public class RedisService : ICacheService
{
    
    private readonly IDistributedCache _cache;

    public RedisService(IDistributedCache distributedCache)
    {
        _cache = distributedCache;
    }
    
    public async Task<T?> GetAsync<T>(string key)
    {
        var cachedData = await _cache.GetAsync(key);
        if (cachedData is null) return default;

        return JsonSerializer.Deserialize<T>(cachedData);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        var options = new DistributedCacheEntryOptions();

        if (absoluteExpiration.HasValue)
            options.AbsoluteExpirationRelativeToNow = absoluteExpiration;

        if (slidingExpiration.HasValue)
            options.SlidingExpiration = slidingExpiration;

        var serializedData = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, serializedData, options);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}