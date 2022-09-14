using System;
using Core.Infrastructure.LogSinks;
using Core.Infrastructure.Services;
using Core.Infrastructure.Stores;
using Core.Services.FileService;
using Core.Services.FileService.UrlStoreFileService;
using Core.Services.ParserService;
using Core.Services.ParserService.UrlStoreParser;
using Microsoft.Extensions.DependencyInjection;


namespace Core.IOC;

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
            .AddTransient<ServiceUrlStoreFileService>();
}