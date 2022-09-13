using Core.Infrastructure.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Core.IOC;

public static partial class IocRegistrator
{
    /// <summary>
    ///     Stores regitrator in DI container
    /// </summary>
    public static IServiceCollection StoresRegistration(this IServiceCollection services) =>
        services
            .AddSingleton<LogsStore>();
}