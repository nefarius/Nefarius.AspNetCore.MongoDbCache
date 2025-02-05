using System;
using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson.Serialization.Attributes;

namespace Nefarius.AspNetCore.MongoDbCache;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
internal class CacheItem
{
    [BsonConstructor]
    public CacheItem(string key, byte[] value, DateTimeOffset? expiresAt, DateTimeOffset? absoluteExpiration,
        double? slidingExpirationInSeconds)
    {
        Key = key;
        Value = value;
        ExpiresAt = expiresAt;
        AbsoluteExpiration = absoluteExpiration;
        SlidingExpirationInSeconds = slidingExpirationInSeconds;
    }

    [BsonConstructor]
    public CacheItem(string key, DateTimeOffset? expiresAt, DateTimeOffset? absoluteExpiration,
        double? slidingExpirationInSeconds)
        : this(key, null, expiresAt, absoluteExpiration, slidingExpirationInSeconds)
    {
    }

    [BsonId] public string Key { get; }

    [BsonElement("value")] public byte[] Value { get; }

    [BsonElement("expiresAt")] public DateTimeOffset? ExpiresAt { get; private set; }

    [BsonElement("absoluteExpiration")] public DateTimeOffset? AbsoluteExpiration { get; }

    [BsonElement("slidingExpirationInSeconds")]
    public double? SlidingExpirationInSeconds { get; }

    public CacheItem WithExpiresAt(DateTimeOffset? expiresAt)
    {
        ExpiresAt = expiresAt;
        return this;
    }
}