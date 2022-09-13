using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using AppInfrastructure.Stores.DefaultStore;
using Core.Models;
using Core.Services.ParserService.UrlStoreParser;
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
    
    public ObservableCollection<ServiceUrl> ServiceUrls { get; private set; }

    #endregion
    
    #region Constructors
    
    public MainWindowVmd(
        IStore<ObservableCollection<string>> loggerStore,
        IStore<ObservableCollection<ServiceUrl>> serviceUrlStore
        ,IStoreParser<string> tagParser)
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
        
        CancelParsingCommand = ReactiveCommand.Create(
            () => { },
            StartParsingCommand.IsExecuting);
        
        #endregion

        serviceUrlStore.CurrentValue = new ObservableCollection<ServiceUrl>
        {
            new ServiceUrl{Path = "https://www.dfsf.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.coost/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "htt://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.yoabr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://www.youtube.com/"},
            new ServiceUrl{Path = "https://habr.com/ru/post/424873/"},
            new ServiceUrl{Path = "https://github.com/" },
            new ServiceUrl{Path = "https://github.com/" }
        };
        //
        // tagParser.Parse("a");

    }

    #endregion


    #region Commands

    public IReactiveCommand StartParsingCommand { get; private set; }
    
    public ReactiveCommand<Unit,Unit> CancelParsingCommand { get; private set; }

    #endregion
    
}