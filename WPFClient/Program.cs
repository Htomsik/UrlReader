using System;
using Core.Infrastructure.LogSinks;
using Core.IOC;
using Core.VMDs.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using UrlReader.VW;

namespace UrlReader;

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
                .WriteTo.Sink(services.GetRequiredService<InformationToLogStoreSink>(),LogEventLevel.Warning);
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