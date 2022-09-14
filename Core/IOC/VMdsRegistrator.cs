using Core.Infrastructure.Stores;
using Core.Services.FileService.UrlStoreFileService;
using Core.Services.ParserService.UrlStoreParser;
using Core.Services.StatisticService;
using Core.Stores;
using Core.VMDs.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Core.IOC;

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
                    s.GetRequiredService<ServiceUrlStoreFileService>(),
                    s.GetRequiredService<ServiceUrlStoreStatisticService>()));
}