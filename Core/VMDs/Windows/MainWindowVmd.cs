using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using AppInfrastructure.Stores.DefaultStore;
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

    #endregion
    
    #region Constructors
    
    public MainWindowVmd(IStore<ObservableCollection<string>> loggerStore)
    {
        #region Subscriptions

        loggerStore.CurrentValueChangedNotifier += () => LastLog = loggerStore.CurrentValue.Last();

        // Will set LatLog to null after 6 seconds after changing
        this.WhenAnyValue(x => x.LastLog)
            .Throttle(TimeSpan.FromSeconds(6))
            .Subscribe(_ => LastLog = string.Empty); 

        #endregion

     
        
    }

    #endregion
    
}