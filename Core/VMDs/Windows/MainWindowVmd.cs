﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using AppInfrastructure.Stores.DefaultStore;
using Core.Models;
using Core.Services.FileService.UrlStoreFileService;
using Core.Services.ParserService.UrlStoreParser;
using Microsoft.Win32;
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
        #endregion

        #region Commands Initializing
        
        StartParsingCommand = ReactiveCommand.CreateFromObservable(
            ()=>
                Observable
                    .StartAsync(ct=>tagParser.Parse("a",ct))
                    .TakeUntil(CancelParsingCommand));

        OpenFileCommand = ReactiveCommand.Create(serviceUrlsStoreFileService.GetDataFromFile, StartParsingCommand.IsExecuting.Select(x=> x == false));
        
        CancelParsingCommand = ReactiveCommand.Create(
            () => { },
            StartParsingCommand.IsExecuting);
        
        #endregion

    }

    #endregion
    
    #region Commands

    /// <summary>
    ///     Start parsing store service command
    /// </summary>
    public IReactiveCommand StartParsingCommand { get;  }
    
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