using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using AppInfrastructure.Stores.DefaultStore;
using Core.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.VMDs.Windows;

/// <summary>
///     Vmd for MainWindow
/// </summary>
public sealed class MainWindowVmd : ReactiveObject
{
    #region Properties and Fields

    /// <summary>
    ///     Last log from loggerStore
    /// </summary>
  
    [Reactive]
    public string LastLog { get; private set; }
    
    public ObservableCollection<ServiceUrl> ServiceUrls { get; }

    #endregion
    
    #region Constructors
    
    public MainWindowVmd(
        IStore<ObservableCollection<string>> loggerStore,
        IStore<ObservableCollection<ServiceUrl>> serviceUrlStore )
    {
        #region Properties and Fields Initializing

        ServiceUrls = serviceUrlStore.CurrentValue;

        #endregion
        
        #region Subscriptions

        loggerStore.CurrentValueChangedNotifier += () => LastLog = loggerStore.CurrentValue.Last();

        // Will set LatLog to null after 6 seconds after changing
        this.WhenAnyValue(x => x.LastLog)
            .Throttle(TimeSpan.FromSeconds(6))
            .Subscribe(_ => LastLog = string.Empty); 

        #endregion
        
        serviceUrlStore.CurrentValue.Add(new ServiceUrl
        {
            Path = "123"
        });
        
    }

    #endregion
    
}