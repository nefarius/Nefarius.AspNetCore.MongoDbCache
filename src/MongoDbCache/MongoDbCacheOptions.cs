using System;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

namespace Nefarius.AspNetCore.MongoDbCache;

/// <inheritdoc />
public class MongoDbCacheOptions : IOptions<MongoDbCacheOptions>
{
    /// <summary>
    ///     The connection string to the database instance.
    /// </summary>
    public string ConnectionString { get; set; }
    
    /// <summary>
    ///     Optional <see cref="MongoClientSettings"/>.
    /// </summary>
    public MongoClientSettings MongoClientSettings { get; set; }
    
    /// <summary>
    ///     The database to store the cache data in.
    /// </summary>
    public string DatabaseName { get; set; }
    
    /// <summary>
    ///     The collection to store the cache data in.
    /// </summary>
    public string CollectionName { get; set; }
    
    /// <summary>
    ///     The interval in which to run cache expiration clean-up.
    /// </summary>
    public TimeSpan? ExpiredScanInterval { get; set; }

    MongoDbCacheOptions IOptions<MongoDbCacheOptions>.Value => this;
}