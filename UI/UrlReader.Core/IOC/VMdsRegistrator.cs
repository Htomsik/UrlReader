using Microsoft.Extensions.DependencyInjection;
using UrlReader.Core.Infrastructure.Stores;
using UrlReader.Core.Services.FileService.UrlStoreFileService;
using UrlReader.Core.Services.ParserService.UrlStoreParser;
using UrlReader.Core.Services.StatisticService;
using UrlReader.Core.Stores;
using UrlReader.Core.VMDs.Windows;

namespace UrlReader.Core.IOC;

public static partial class IocRegistrator
{
    /// <summary>
    ///     Vmds regitrator in DI container
    /// </summary>
    public static IServiceCollection VmdsRegistration(this IServiceCollection services) => 
        services
            .AddTransient<MainWindowVmd>(s=> 
                new (s.GetRequiredService<LogsStore>(),
                    s.GetRequiredService<ServiceUrlStore>(),
                    s.GetRequiredService<ServiceUrlStoreTagParser>(),
                    s.GetRequiredService<ServiceUrlCollectionStoreFileService>(),
                    s.GetRequiredService<ServiceUrlStoreStatisticService>()));
}