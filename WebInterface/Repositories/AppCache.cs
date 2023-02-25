using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace WebInterface.Repositories;

public class AppCache
{
    /// <summary>
    /// Distributed cache.
    /// </summary>
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Instance of logger.
    /// </summary>
    private readonly ILogger<AppCache> _logger;

    public AppCache(IDistributedCache cache, ILogger<AppCache> logger)
	{
        _cache = cache;
        _logger = logger;
    }

    public async Task<string?> GetStringAsync(string key)
    {
        try
        {
            return await _cache.GetStringAsync(key);
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Unable to connect to the Redis server(s).");
        }
        return null;
    }

    public async Task SetStringAsync(
        string key, string value, DistributedCacheEntryOptions options)
    {
        try
        {
            await _cache.SetStringAsync(key, value, options);
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Unable to connect to the Redis server(s).");
        }
    }
}
