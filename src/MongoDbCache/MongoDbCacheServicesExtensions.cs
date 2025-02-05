using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Nefarius.AspNetCore.MongoDbCache;

/// <summary>
///     <see cref="IServiceCollection" /> extensions.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class MongoDbCacheServicesExtensions
{
    /// <summary>
    ///     Adds MongoDb distributed caching services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="setupAction">
    ///     An <see cref="Action{MongoDbCacheOptions}" /> to configure the provided
    ///     <see cref="MongoDbCacheOptions" />.
    /// </param>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public static IServiceCollection AddMongoDbCache(this IServiceCollection services,
        Action<MongoDbCacheOptions> setupAction)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (setupAction == null)
        {
            throw new ArgumentNullException(nameof(setupAction));
        }

        services.AddOptions();
        services.Configure(setupAction);
        services.AddSingleton<IDistributedCache, MongoDbCache>();

        return services;
    }

    /// <summary>
    ///     Adds MongoDb distributed caching services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="setupAction">
    ///     An <see cref="Action{MongoDbCacheOptions,IServiceProvider}" /> to configure the provided
    ///     <see cref="MongoDbCacheOptions" />.
    /// </param>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    public static IServiceCollection AddMongoDbCache(
        this IServiceCollection services,
        Action<MongoDbCacheOptions, IServiceProvider> setupAction)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (setupAction == null)
        {
            throw new ArgumentNullException(nameof(setupAction));
        }

        services.AddOptions();
        services.AddSingleton<IConfigureOptions<MongoDbCacheOptions>>(sp =>
            new ConfigureOptions<MongoDbCacheOptions>(options => setupAction(options, sp))
        );
        services.TryAddSingleton<IDistributedCache, MongoDbCache>();

        return services;
    }
}