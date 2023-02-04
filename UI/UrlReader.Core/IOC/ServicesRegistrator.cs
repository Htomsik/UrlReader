using System;
using Microsoft.Extensions.DependencyInjection;
using UrlReader.Core.Infrastructure.LogSinks;
using UrlReader.Core.Infrastructure.Services;
using UrlReader.Core.Infrastructure.Stores;
using UrlReader.Core.Services.FileService.Base;
using UrlReader.Core.Services.FileService.UrlStoreFileService;
using UrlReader.Core.Services.ParserService.Base;
using UrlReader.Core.Services.ParserService.UrlStoreParser;
using UrlReader.Core.Services.StatisticService;

namespace UrlReader.Core.IOC;

/// <summary>
///     DI registrator
/// </summary>
public static partial class IocRegistrator
{
    /// <summary>
    ///     Service regitrator in DI container
    /// </summary>
    public static IServiceCollection ServiceRegistration(this IServiceCollection services) =>
        services
            .AddSingleton<IObserver<Exception>, GlobalExceptionHandler>()
            .AddSingleton(s => new InformationToLogStoreSink(s.GetRequiredService<LogsStore>()))
            .AddTransient<TagParser>()
            .AddTransient<ServiceUrlStoreTagParser>()
            .AddTransient<JsonClientFileService>()
            .AddTransient<ServiceUrlCollectionStoreFileService>()
            .AddTransient<ServiceUrlStoreStatisticService>();
}