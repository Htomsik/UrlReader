using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using AppInfrastructure.Stores.DefaultStore;
using Core.Models;
using Core.Services.FileService.UrlStoreFileService;
using Core.Services.ParserService.UrlStoreParser;
using DynamicData.Binding;
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
    
    [Reactive]
    public ObservableCollection<ServiceUrl> ServiceUrls { get; private set; }

    #region ServiceUrls Stats

    [Reactive]
    public int AlivesUrlsCount { get; private set; }
    
    [Reactive]
    public int NotAlivesUrlsCount { get; private set; }
    
    [Reactive]
    public int UnknownUrlsCount { get; private set; }

    #endregion

    #endregion
    
    #region Constructors
    
    public MainWindowVmd(
        IStore<ObservableCollection<string>> loggerStore,
        IStore<ObservableCollection<ServiceUrl>> serviceUrlStore,
        IStoreParser<string> tagParser,
        IStoreFileService serviceUrlsStoreFileService)
    {
        #region Properties and Fields Initializing

        ServiceUrls = serviceUrlStore.CurrentValue;

        #endregion
        
        #region Subscriptions

        serviceUrlStore.CurrentValueChangedNotifier += () => ServiceUrls = serviceUrlStore.CurrentValue;
        
        loggerStore.CurrentValueChangedNotifier += () => LastLog = loggerStore.CurrentValue.Last();

        // Will set LastLog to null after 6 seconds after changing
        this.WhenAnyValue(x => x.LastLog)
            .Throttle(TimeSpan.FromSeconds(6))
            .Subscribe(_ => LastLog = string.Empty);

        // Urls stats updater
        this.WhenAnyPropertyChanged()
            .Subscribe(_ =>
            {
                AlivesUrlsCount = ServiceUrls.Count(x => x.State == UrlState.Alive);
                NotAlivesUrlsCount = ServiceUrls.Count(x => x.State == UrlState.NotAlive);
                UnknownUrlsCount = ServiceUrls.Count(x => x.State == UrlState.Unknown);
            });
        
        #endregion

        #region Commands Initializing
        
        StartParsingCommand = ReactiveCommand.CreateFromObservable(
            ()=>
                Observable
                    .StartAsync(ct=>tagParser.Parse("a",ct))
                    .TakeUntil(CancelParsingCommand),CanStartParsing);

        OpenFileCommand = ReactiveCommand.Create(serviceUrlsStoreFileService.GetDataFromFile, StartParsingCommand.IsExecuting.Select(x=> x == false));
        
        CancelParsingCommand = ReactiveCommand.Create(
            () => { },
            StartParsingCommand.IsExecuting);
        
        #endregion

    }

    #endregion
    
    #region Commands
    
    #region StartParsingCommand :  Start parsing store service command

    /// <summary>
    ///     Start parsing store service command
    /// </summary>
    public IReactiveCommand StartParsingCommand { get;  }

    private IObservable<bool> CanStartParsing => 
        this.WhenAnyValue(x => x.ServiceUrls,(urls => urls.Count != 0 ));

    #endregion
    
    
    /// <summary>
    ///     Open json file command
    /// </summary>
    public ReactiveCommand<Unit,Unit> OpenFileCommand { get; }
    
    /// <summary>
    ///     Cancel StartParsingCommand
    /// </summary>
    public ReactiveCommand<Unit,Unit> CancelParsingCommand { get;  }

    #endregion
    
}