using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;

using MongoDB.Driver;

namespace Nefarius.AspNetCore.MongoDbCache;

internal class MongoContext
{
    private readonly IMongoCollection<CacheItem> _collection;

    public MongoContext(string connectionString, MongoClientSettings mongoClientSettings, string databaseName,
        string collectionName)
    {
        MongoClient client = mongoClientSettings == null
            ? new MongoClient(connectionString)
            : new MongoClient(mongoClientSettings);
        IMongoDatabase database = client.GetDatabase(databaseName);

        IndexKeysDefinition<CacheItem> expireAtIndexModel =
            new IndexKeysDefinitionBuilder<CacheItem>().Ascending(p => p.ExpiresAt);

        _collection = database.GetCollection<CacheItem>(collectionName);

        _collection.Indexes.CreateOne(new CreateIndexModel<CacheItem>(expireAtIndexModel,
            new CreateIndexOptions { Background = true }));
    }

    private static FilterDefinition<CacheItem> FilterByKey(string key)
    {
        return Builders<CacheItem>.Filter.Eq(x => x.Key, key);
    }

    private static FilterDefinition<CacheItem> FilterByExpiresAtNotNull()
    {
        return Builders<CacheItem>.Filter.Ne(x => x.ExpiresAt, null);
    }

    private IFindFluent<CacheItem, CacheItem> GetItemQuery(string key, bool withoutValue)
    {
        IFindFluent<CacheItem, CacheItem> query = _collection.Find(FilterByKey(key));
        if (withoutValue)
        {
            query = query.Project<CacheItem>(Builders<CacheItem>.Projection.Exclude(x => x.Value));
        }

        return query;
    }

    private static bool CheckIfExpired(DateTimeOffset utcNow, CacheItem cacheItem)
    {
        return cacheItem?.ExpiresAt <= utcNow;
    }

    private static DateTimeOffset? GetExpiresAt(DateTimeOffset utcNow, double? slidingExpirationInSeconds,
        DateTimeOffset? absoluteExpiration)
    {
        if (slidingExpirationInSeconds == null && absoluteExpiration == null)
        {
            return null;
        }

        if (slidingExpirationInSeconds == null)
        {
            return absoluteExpiration;
        }

        double seconds = slidingExpirationInSeconds.GetValueOrDefault();

        return utcNow.AddSeconds(seconds) > absoluteExpiration
            ? absoluteExpiration
            : utcNow.AddSeconds(seconds);
    }

    private CacheItem UpdateExpiresAtIfRequired(DateTimeOffset utcNow, CacheItem cacheItem)
    {
        if (cacheItem.ExpiresAt == null)
        {
            return cacheItem;
        }

        DateTimeOffset? absoluteExpiration =
            GetExpiresAt(utcNow, cacheItem.SlidingExpirationInSeconds, cacheItem.AbsoluteExpiration);
        _collection.UpdateOne(FilterByKey(cacheItem.Key) & FilterByExpiresAtNotNull(),
            Builders<CacheItem>.Update.Set(x => x.ExpiresAt, absoluteExpiration));

        return cacheItem.WithExpiresAt(absoluteExpiration);
    }

    private async Task<CacheItem> UpdateExpiresAtIfRequiredAsync(DateTimeOffset utcNow, CacheItem cacheItem)
    {
        if (cacheItem.ExpiresAt == null)
        {
            return cacheItem;
        }

        DateTimeOffset? absoluteExpiration =
            GetExpiresAt(utcNow, cacheItem.SlidingExpirationInSeconds, cacheItem.AbsoluteExpiration);
        await _collection.UpdateOneAsync(FilterByKey(cacheItem.Key) & FilterByExpiresAtNotNull(),
            Builders<CacheItem>.Update.Set(x => x.ExpiresAt, absoluteExpiration));

        return cacheItem.WithExpiresAt(absoluteExpiration);
    }

    public void DeleteExpired(DateTimeOffset utcNow)
    {
        _collection.DeleteMany(Builders<CacheItem>.Filter.Lte(x => x.ExpiresAt, utcNow));
    }

    public byte[] GetCacheItem(string key, bool withoutValue)
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;

        if (key == null)
        {
            return null;
        }

        IFindFluent<CacheItem, CacheItem> query = GetItemQuery(key, withoutValue);
        CacheItem cacheItem = query.SingleOrDefault();
        if (cacheItem == null)
        {
            return null;
        }

        if (CheckIfExpired(utcNow, cacheItem))
        {
            Remove(cacheItem.Key);
            return null;
        }

        cacheItem = UpdateExpiresAtIfRequired(utcNow, cacheItem);

        return cacheItem?.Value;
    }

    public async Task<byte[]> GetCacheItemAsync(string key, bool withoutValue, CancellationToken token = default)
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;

        if (key == null)
        {
            return null;
        }

        IFindFluent<CacheItem, CacheItem> query = GetItemQuery(key, withoutValue);
        CacheItem cacheItem = await query.SingleOrDefaultAsync(token);
        if (cacheItem == null)
        {
            return null;
        }

        if (CheckIfExpired(utcNow, cacheItem))
        {
            await RemoveAsync(cacheItem.Key, token);
            return null;
        }

        cacheItem = await UpdateExpiresAtIfRequiredAsync(utcNow, cacheItem);

        return cacheItem?.Value;
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options = null)
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;

        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        DateTimeOffset? absolutExpiration = options?.AbsoluteExpiration;
        double? slidingExpirationInSeconds = options?.SlidingExpiration?.TotalSeconds;

        if (options?.AbsoluteExpirationRelativeToNow != null)
        {
            absolutExpiration = utcNow.Add(options.AbsoluteExpirationRelativeToNow.Value);
        }

        if (absolutExpiration <= utcNow)
        {
            throw new InvalidOperationException("The absolute expiration value must be in the future.");
        }

        DateTimeOffset? expiresAt = GetExpiresAt(utcNow, slidingExpirationInSeconds, absolutExpiration);
        CacheItem cacheItem = new(key, value, expiresAt, absolutExpiration, slidingExpirationInSeconds);

        _collection.ReplaceOne(FilterByKey(key), cacheItem, new ReplaceOptions { IsUpsert = true });
    }

    public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options = null,
        CancellationToken token = default)
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;

        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        DateTimeOffset? absolutExpiration = options?.AbsoluteExpiration;
        double? slidingExpirationInSeconds = options?.SlidingExpiration?.TotalSeconds;

        if (options?.AbsoluteExpirationRelativeToNow != null)
        {
            absolutExpiration = utcNow.Add(options.AbsoluteExpirationRelativeToNow.Value);
        }

        if (absolutExpiration <= utcNow)
        {
            throw new InvalidOperationException("The absolute expiration value must be in the future.");
        }

        DateTimeOffset? expiresAt = GetExpiresAt(utcNow, slidingExpirationInSeconds, absolutExpiration);
        CacheItem cacheItem = new(key, value, expiresAt, absolutExpiration, slidingExpirationInSeconds);

        await _collection.ReplaceOneAsync(FilterByKey(key), cacheItem, new ReplaceOptions { IsUpsert = true }, token);
    }

    public void Remove(string key)
    {
        _collection.DeleteOne(FilterByKey(key));
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        await _collection.DeleteOneAsync(FilterByKey(key), token);
    }
}