#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Nefarius.AspNetCore.MongoDbCache;

/// <inheritdoc />
public class MongoDbCache : IDistributedCache
{
    private readonly TimeSpan _defaultScanInterval = TimeSpan.FromMinutes(5);
    private readonly MongoContext _mongoContext;
    private DateTimeOffset _lastScan = DateTimeOffset.UtcNow;
    private TimeSpan _scanInterval;

    /// <summary>
    ///     Provides a MongoDB-based implementation of <see cref="IDistributedCache" />.
    /// </summary>
    public MongoDbCache(IOptions<MongoDbCacheOptions> optionsAccessor)
    {
        MongoDbCacheOptions options = optionsAccessor.Value;
        ValidateOptions(options);

        _mongoContext = new MongoContext(options.ConnectionString, options.MongoClientSettings,
            options.DatabaseName, options.CollectionName);

        SetScanInterval(options.ExpiredScanInterval);
    }

    /// <inheritdoc />
    public byte[] Get(string key)
    {
        byte[] value = _mongoContext.GetCacheItem(key, false);

        ScanAndDeleteExpired();

        return value;
    }

    /// <inheritdoc />
    public void Set(string key, byte[] value, DistributedCacheEntryOptions? options = null)
    {
        _mongoContext.Set(key, value, options);

        ScanAndDeleteExpired();
    }

    /// <inheritdoc />
    public void Refresh(string key)
    {
        _mongoContext.GetCacheItem(key, true);

        ScanAndDeleteExpired();
    }

    /// <inheritdoc />
    public async Task<byte[]?> GetAsync(string key, CancellationToken token = default)
    {
        byte[] value = await _mongoContext.GetCacheItemAsync(key, false, token);

        ScanAndDeleteExpired();

        return value;
    }

    /// <inheritdoc />
    public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
        CancellationToken token = default)
    {
        await _mongoContext.SetAsync(key, value, options, token);

        ScanAndDeleteExpired();
    }

    /// <inheritdoc />
    public async Task RefreshAsync(string key, CancellationToken token = default)
    {
        await _mongoContext.GetCacheItemAsync(key, true, token);

        ScanAndDeleteExpired();
    }

    /// <inheritdoc />
    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        await _mongoContext.RemoveAsync(key, token);

        ScanAndDeleteExpired();
    }

    /// <inheritdoc />
    public void Remove(string key)
    {
        _mongoContext.Remove(key);

        ScanAndDeleteExpired();
    }

    private static void ValidateOptions(MongoDbCacheOptions cacheOptions)
    {
        if (!string.IsNullOrEmpty(cacheOptions.ConnectionString) && cacheOptions.MongoClientSettings != null)
        {
            throw new ArgumentException(
                $"Only one of {nameof(cacheOptions.ConnectionString)} and {nameof(cacheOptions.MongoClientSettings)} can be set.");
        }

        if (string.IsNullOrEmpty(cacheOptions.ConnectionString) && cacheOptions.MongoClientSettings == null)
        {
            throw new ArgumentException(
                $"{nameof(cacheOptions.ConnectionString)} or {nameof(cacheOptions.MongoClientSettings)} cannot be empty or null.");
        }

        if (string.IsNullOrEmpty(cacheOptions.DatabaseName))
        {
            throw new ArgumentException($"{nameof(cacheOptions.DatabaseName)} cannot be empty or null.");
        }

        if (string.IsNullOrEmpty(cacheOptions.CollectionName))
        {
            throw new ArgumentException($"{nameof(cacheOptions.CollectionName)} cannot be empty or null.");
        }
    }

    private void SetScanInterval(TimeSpan? scanInterval)
    {
        _scanInterval = scanInterval?.TotalSeconds > 0
            ? scanInterval.Value
            : _defaultScanInterval;
    }

    private void ScanAndDeleteExpired()
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;

        if (_lastScan.Add(_scanInterval) < utcNow)
        {
            Task.Run(() =>
            {
                _lastScan = utcNow;
                _mongoContext.DeleteExpired(utcNow);
            });
        }
    }
}