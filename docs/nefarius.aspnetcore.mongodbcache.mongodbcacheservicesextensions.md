# MongoDbCacheServicesExtensions

Namespace: Nefarius.AspNetCore.MongoDbCache

extensions.

```csharp
public static class MongoDbCacheServicesExtensions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [MongoDbCacheServicesExtensions](./nefarius.aspnetcore.mongodbcache.mongodbcacheservicesextensions.md)

## Methods

### <a id="methods-addmongodbcache"/>**AddMongoDbCache(IServiceCollection, Action&lt;MongoDbCacheOptions&gt;)**

Adds MongoDb distributed caching services to the specified .

```csharp
public static IServiceCollection AddMongoDbCache(IServiceCollection services, Action<MongoDbCacheOptions> setupAction)
```

#### Parameters

`services` IServiceCollection<br>
The  to add services to.

`setupAction` [Action&lt;MongoDbCacheOptions&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
An [Action&lt;T&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1) to configure the provided
 [MongoDbCacheOptions](./nefarius.aspnetcore.mongodbcache.mongodbcacheoptions.md).

#### Returns

The  so that additional calls can be chained.

### <a id="methods-addmongodbcache"/>**AddMongoDbCache(IServiceCollection, Action&lt;MongoDbCacheOptions, IServiceProvider&gt;)**

Adds MongoDb distributed caching services to the specified .

```csharp
public static IServiceCollection AddMongoDbCache(IServiceCollection services, Action<MongoDbCacheOptions, IServiceProvider> setupAction)
```

#### Parameters

`services` IServiceCollection<br>
The  to add services to.

`setupAction` [Action&lt;MongoDbCacheOptions, IServiceProvider&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-2)<br>
An [Action&lt;T1, T2&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-2) to configure the provided
 [MongoDbCacheOptions](./nefarius.aspnetcore.mongodbcache.mongodbcacheoptions.md).

#### Returns

The  so that additional calls can be chained.
