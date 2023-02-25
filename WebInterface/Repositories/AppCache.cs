﻿using Microsoft.Extensions.Caching.Distributed;
using NLog;
using StackExchange.Redis;

namespace WebInterface.Repositories;

public class AppCache
{
    private readonly IDistributedCache _cache;

    private readonly Logger _logger;

    public AppCache(IDistributedCache cache)
	{
        _cache = cache;
        // ToDo:
        // Use DI.
        _logger = LogManager.GetCurrentClassLogger();
    }

    public async Task<string?> GetStringAsync(string key)
    {
        try
        {
            return await _cache.GetStringAsync(key);
        }
        catch (RedisConnectionException e)
        {
            _logger.Warn($"Unable to connect to the Redis server(s).");
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
        catch (RedisConnectionException e)
        {
            _logger.Warn($"Unable to connect to the Redis server(s).");
        }
    }
}