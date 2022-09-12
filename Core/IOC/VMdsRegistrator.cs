using Core.VMDs.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Core.IOC;

public static partial class IocRegistrator
{
    /// <summary>
    ///     Vmds regitrator in DI container
    /// </summary>
    public static IServiceCollection VmdsRegistration(this IServiceCollection services) => services.AddTransient<MainWindowVmd>();
}