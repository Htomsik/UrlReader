using Microsoft.Extensions.DependencyInjection;
using UrlReader.Core.Infrastructure.Stores;
using UrlReader.Core.Stores;

namespace UrlReader.Core.IOC;

public static partial class IocRegistrator
{
    /// <summary>
    ///     Stores regitrator in DI container
    /// </summary>
    public static IServiceCollection StoresRegistration(this IServiceCollection services) =>
        services
            .AddSingleton<LogsStore>()
            .AddSingleton<ServiceUrlStore>()
            .AddSingleton<HttpClientStore>();
}