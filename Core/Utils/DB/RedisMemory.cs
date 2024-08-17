using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Core.Config;
using Core.Utils.Security;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Core.Utils.DB;

[ExcludeFromCodeCoverage]
public class RedisMemory : ICache
{
    private readonly CacheConfig _cacheConfig;
    private readonly ActivitySource _activitySource = new("cache");
    private readonly IDatabase _database;
    private readonly ILogger<RedisMemory> _logger;

    public RedisMemory(
        IOptions<CacheConfig> cacheConfig,
        IVault vault,
        ILogger<RedisMemory> logger)
    {
        _logger = logger;
        _cacheConfig = vault.RevealSecret(cacheConfig.Value);
        var connectionMultiplexer = ConnectionMultiplexer.Connect($"{_cacheConfig.Server}:{_cacheConfig.Port}");
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task<string?> GetStringAsync(string key)
    {
        using var activity = _activitySource.StartActivity();
        var data = await _database.StringGetAsync(key);
        _logger.LogDebug("Cache GetStringAsync: {key} {data}", key, data);
        activity?.SetTag("key", key);
        return data;
    }

    public async Task SetStringAsync(string key, string value)
    {
        using var activity = _activitySource.StartActivity();
        await _database.StringSetAsync(key, value, TimeSpan.FromMinutes(_cacheConfig.DurationMinutes));
        _logger.LogDebug("Cache SetStringAsync: {key}", key);
        activity?.SetTag("key", key);
        activity?.SetTag("DurationMinutes", _cacheConfig.DurationMinutes);
    }

    public async Task RemoveAsync(string key)
    {
        using var activity = _activitySource.StartActivity();
        await _database.KeyDeleteAsync(key);
        _logger.LogDebug("Cache RemoveAsync: {key}", key);
        activity?.SetTag("key", key);
    }

    public string? GetString(string key)
    {
        using var activity = _activitySource.StartActivity();
        var data = _database.StringGet(key);
        _logger.LogDebug("Cache GetString: {key}", key);
        activity?.SetTag("key", key);
        return data;
    }

    public void SetString(string key, string value)
    {
        using var activity = _activitySource.StartActivity();
        _database.StringSet(key, value, TimeSpan.FromMinutes(_cacheConfig.DurationMinutes));
        _logger.LogDebug("Cache SetString: {key}", key );
        activity?.SetTag("key", key);
        activity?.SetTag("DurationMinutes", _cacheConfig.DurationMinutes);
    }

    public void Remove(string key)
    {
        using var activity = _activitySource.StartActivity(ActivityKind.Client);
        _database.KeyDelete(key);
        _logger.LogDebug("Cache Remove: {key}", key);
        activity?.SetTag("key", key);
    }
}