using System;
using Core.IOC;
using Core.VMDs.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

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
        .UseSerilog((host, loggerConfiguration) =>
        {
            loggerConfiguration
                .WriteTo.File(@"logs\Log-.txt", rollingInterval: RollingInterval.Day);
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