using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using UrlReader.Core.Infrastructure.LogSinks;
using UrlReader.Core.IOC;
using UrlReader.Core.VMDs.Windows;
using UrlReader.WPF.VW;

namespace UrlReader.WPF;

public class Program
{
    [STAThread]
    public static void Main()
    {
        var app = new App();

        app.InitializeComponent();

        app.Run();
    }


    #region Methods

    internal static IHostBuilder CreateHostBuilder(string[] args)
    => Host
        .CreateDefaultBuilder(args)
        .ConfigureServices(ConfigureServices)
        .UseSerilog((context, services, configuration) =>
        {
            configuration
                .WriteTo.File(@"logs\Log-.txt", rollingInterval: RollingInterval.Day,restrictedToMinimumLevel: LogEventLevel.Warning)
                .WriteTo.Sink(services.GetRequiredService<InformationToLogStoreSink>(),LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
        });
    

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        => services
            .StoresRegistration()
            .ServiceRegistration()
            .VmdsRegistration()
            .AddSingleton(s => new MainWindow
            {
                DataContext = s.GetRequiredService<MainWindowVmd>()
            });
    
    #endregion


}