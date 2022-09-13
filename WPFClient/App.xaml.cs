using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using UrlReader.VW;

namespace UrlReader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Properties and Fileds

        #region Host

        private static IHost? _host;

        public static IHost Host
            => _host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        #endregion

        public static IServiceProvider Services => Host.Services;

        #endregion

        #region Methods
        
        private void SetupGlobalExceptionHandler() =>  RxApp.DefaultExceptionHandler = Services.GetRequiredService<IObserver<Exception>>();
        
        #region OnStratup

        protected override async void OnStartup(StartupEventArgs e)
        {
            SetupGlobalExceptionHandler();
            
            base.OnStartup(e);

            MainWindow = Services.GetRequiredService<MainWindow>();

            MainWindow.Show();
            
           await Host.StartAsync();
        }

        #endregion

        #region OnExit

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            await Host.StopAsync();
            
            Host.Dispose();
        }

        #endregion
        
    

        #endregion
    }
}