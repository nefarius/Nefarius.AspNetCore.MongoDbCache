# MongoDbCache

Namespace: Nefarius.AspNetCore.MongoDbCache

```csharp
public class MongoDbCache : Microsoft.Extensions.Caching.Distributed.IDistributedCache
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [MongoDbCache](./nefarius.aspnetcore.mongodbcache.mongodbcache.md)<br>
Implements IDistributedCache

## Constructors

### <a id="constructors-.ctor"/>**MongoDbCache(IOptions&lt;MongoDbCacheOptions&gt;)**

Provides a MongoDB-based implementation of .

```csharp
public MongoDbCache(IOptions<MongoDbCacheOptions> optionsAccessor)
```

#### Parameters

`optionsAccessor` IOptions&lt;MongoDbCacheOptions&gt;<br>

## Methods

### <a id="methods-get"/>**Get(String)**

```csharp
public Byte[] Get(string key)
```

#### Parameters

`key` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[Byte[]](https://docs.microsoft.com/en-us/dotnet/api/system.byte)

### <a id="methods-getasync"/>**GetAsync(String, CancellationToken)**

```csharp
public Task<Byte[]> GetAsync(string key, CancellationToken token)
```

#### Parameters

`key` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`token` [CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken)<br>

#### Returns

[Task&lt;Byte[]&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1)

### <a id="methods-refresh"/>**Refresh(String)**

```csharp
public void Refresh(string key)
```

#### Parameters

`key` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="methods-refreshasync"/>**RefreshAsync(String, CancellationToken)**

```csharp
public Task RefreshAsync(string key, CancellationToken token)
```

#### Parameters

`key` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`token` [CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken)<br>

#### Returns

[Task](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task)

### <a id="methods-remove"/>**Remove(String)**

```csharp
public void Remove(string key)
```

#### Parameters

`key` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="methods-removeasync"/>**RemoveAsync(String, CancellationToken)**

```csharp
public Task RemoveAsync(string key, CancellationToken token)
```

#### Parameters

`key` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`token` [CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken)<br>

#### Returns

[Task](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task)

### <a id="methods-set"/>**Set(String, Byte[], DistributedCacheEntryOptions)**

```csharp
public void Set(string key, Byte[] value, DistributedCacheEntryOptions options)
```

#### Parameters

`key` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`value` [Byte[]](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>

`options` DistributedCacheEntryOptions<br>

### <a id="methods-setasync"/>**SetAsync(String, Byte[], DistributedCacheEntryOptions, CancellationToken)**

```csharp
public Task SetAsync(string key, Byte[] value, DistributedCacheEntryOptions options, CancellationToken token)
```

#### Parameters

`key` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`value` [Byte[]](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>

`options` DistributedCacheEntryOptions<br>

`token` [CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken)<br>

#### Returns

[Task](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task)
