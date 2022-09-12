using System;
using Core.Services.AppInfrastructure;
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
        services.AddSingleton<IObserver<Exception>,GlobalExceptionHandler>();

}