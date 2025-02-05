# MongoDbCacheOptions

Namespace: Nefarius.AspNetCore.MongoDbCache

```csharp
public class MongoDbCacheOptions : Microsoft.Extensions.Options.IOptions`1[[Nefarius.AspNetCore.MongoDbCache.MongoDbCacheOptions, Nefarius.AspNetCore.MongoDbCache, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null]]
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [MongoDbCacheOptions](./nefarius.aspnetcore.mongodbcache.mongodbcacheoptions.md)<br>
Implements IOptions&lt;MongoDbCacheOptions&gt;

## Properties

### <a id="properties-collectionname"/>**CollectionName**

The collection to store the cache data in.

```csharp
public string CollectionName { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-connectionstring"/>**ConnectionString**

The connection string to the database instance.

```csharp
public string ConnectionString { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-databasename"/>**DatabaseName**

The database to store the cache data in.

```csharp
public string DatabaseName { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-expiredscaninterval"/>**ExpiredScanInterval**

The interval in which to run cache expiration clean-up.

```csharp
public Nullable<TimeSpan> ExpiredScanInterval { get; set; }
```

#### Property Value

[Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

### <a id="properties-mongoclientsettings"/>**MongoClientSettings**

Optional [MongoDbCacheOptions.MongoClientSettings](./nefarius.aspnetcore.mongodbcache.mongodbcacheoptions.md#mongoclientsettings).

```csharp
public MongoClientSettings MongoClientSettings { get; set; }
```

#### Property Value

MongoClientSettings<br>

## Constructors

### <a id="constructors-.ctor"/>**MongoDbCacheOptions()**

```csharp
public MongoDbCacheOptions()
```
