using Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

public class HybridCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ICacheService _redisCache;

    public HybridCacheService(
        IMemoryCache memoryCache,
        ICacheService redisCache)
    {
        _memoryCache = memoryCache;
        _redisCache = redisCache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        // 🟢 1. Memory first
        if (_memoryCache.TryGetValue(key, out T? value))
        {
            return value;
        }

        // 🔵 2. Redis second
        var redisValue = await _redisCache.GetAsync<T>(key);

        if (redisValue != null)
        {
            // نخزنه في memory مؤقتًا
            _memoryCache.Set(key, redisValue, TimeSpan.FromMinutes(2));

            return redisValue;
        }

        return default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        // 🟢 Memory
        _memoryCache.Set(key, value, TimeSpan.FromMinutes(2));

        // 🔵 Redis
        await _redisCache.SetAsync(key, value, expiration);
    }

    public async Task RemoveAsync(string key)
    {
        _memoryCache.Remove(key);
        await _redisCache.RemoveAsync(key);
    }
}