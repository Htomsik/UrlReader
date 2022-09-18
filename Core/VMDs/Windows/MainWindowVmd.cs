using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using AppInfrastructure.Services.NavigationServices.Close;
using AppInfrastructure.Stores.DefaultStore;
using Core.Models;
using Core.Services.FileService.UrlStoreFileService;
using Core.Services.ParserService.UrlStoreParser;
using Core.Services.StatisticService;
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
    [Reactive] public string? LastLog { get; private set; }
    
    /// <summary>
    ///     Collection with parsings urls 
    /// </summary>
    [Reactive] public ObservableCollection<ServiceUrl> ServiceUrls { get; private set; }
  
    /// <summary>
    ///     Statistic operations server. Calculate some params about Store with ServiceUrls 
    /// </summary>
    [Reactive] public IUrlsStatisticService UrlsStatisticService { get; init; }
    
    /// <summary>
    ///     Current seleted html tag in HtmlTagsList
    /// </summary>
    [Reactive] public string SelectedHtmlTag { get; set; }
    
    /// <summary>
    ///     Retranslator from StartParsingCommand IsExecuting into bool for use into Xaml 
    /// </summary>
    [Reactive] public bool IsParsingNow { get; private set; }
    
    #region HtmlTagsList :   List with html tags for Parsing service

    /// <summary>
    ///     List with html tags for Parsing service
    /// </summary>
    public List<string> HtmlTagsList => new()
    {
        "a",
        "abbr",
        "address",
        "area",
        "article",
        "aside",
        "audio",
        "b",
        "base",
        "bdi",
        "bdo",
        "blockquote",
        "body",
        "br",
        "button",
        "canvas",
        "caption",
        "cite",
        "code",
        "col",
        "colgroup",
        "data",
        "datalist",
        "dd",
        "del",
        "details",
        "dfn",
        "dialog",
        "div",
        "dl",
        "dt",
        "em",
        "embed",
        "fieldset",
        "figcaption",
        "figure",
        "footer",
        "form",
        "head",
        "header",
        "hr",
        "html",
        "i",
        "iframe",
        "img",
        "input",
        "ins",
        "kbd",
        "label",
        "legend",
        "li",
        "link",
        "main",
        "map",
        "mark",
        "meta",
        "meter",
        "nav",
        "noscript",
        "object",
        "ol",
        "optgroup",
        "option",
        "output",
        "p",
        "param",
        "picture",
        "pre",
        "progress",
        "q",
        "ruby",
        "rb",
        "rt",
        "rtc",
        "rp",
        "s",
        "samp",
        "script",
        "section",
        "select",
        "small",
        "source",
        "span",
        "strong",
        "style",
        "sub",
        "summary",
        "sup",
        "table",
        "tbody",
        "td",
        "template",
        "textarea",
        "tfoot",
        "th",
        "thead",
        "time",
        "title",
        "tr",
        "track",
        "u",
        "ul",
        "var",
        "video",
        "wbr",
    };

    #endregion
    
    #endregion
    
    #region Constructors
    
    public MainWindowVmd(
        IStore<ObservableCollection<string>> loggerStore,
        IStore<ObservableCollection<ServiceUrl>> serviceUrlStore,
        IStoreParser<string> tagParser,
        IStoreFileService serviceUrlsStoreFileService,
        IUrlsStatisticService urlsStatisticService)
    {
        #region Stores and Services Initializing

        UrlsStatisticService = urlsStatisticService;

        #endregion
        
        #region Properties and Fields Initializing

        ServiceUrls = serviceUrlStore.CurrentValue;

        SelectedHtmlTag = HtmlTagsList.First();
        
        #endregion
        
        #region Commands Initializing
        
        StartParsingCommand = ReactiveCommand.CreateFromObservable(
            ()=>
                Observable
                    .StartAsync(ct=>tagParser.Parse(SelectedHtmlTag,ct))
                    .TakeUntil(CancelParsingCommand),CanStartParsing);

        OpenFileCommand = ReactiveCommand.CreateFromObservable(()=>
            Observable
                .StartAsync(ct=> serviceUrlsStoreFileService.GetDataFromFile(ct)).TakeUntil(CancelParsingCommand), StartParsingCommand.IsExecuting.Select(x=> x == false));

        ClearDataCommand = ReactiveCommand.Create(()=> ((ICloseServices)(serviceUrlStore)).Close(),CanClearData);
        
        CancelParsingCommand = ReactiveCommand.Create(() => { },CanCancel);
            
        
        
        #endregion
        
        #region Subscriptions

        serviceUrlStore.CurrentValueChangedNotifier += () => ServiceUrls = serviceUrlStore.CurrentValue;
        
        loggerStore.CurrentValueChangedNotifier += () => 
        {
            if (loggerStore.CurrentValue.Count != 0)
                LastLog = loggerStore.CurrentValue.Last();
           
        };
        
        // Will set LastLog to null after 6 seconds after changing
        this.WhenAnyValue(x => x.LastLog)
            .Throttle(TimeSpan.FromSeconds(6))
            .Subscribe(_ => LastLog = null);
        
        // TagsCounter when change SelectedHtmlTag Updater
        this.WhenAnyValue(x => x.SelectedHtmlTag)
            .Subscribe(_ =>
            {
                foreach (var item in serviceUrlStore.CurrentValue)
                {
                    item.TagsCount = 0;
                    item.State = UrlState.Unknown;
                    item.IsMaxValue = false;
                }
                urlsStatisticService.Close();
            });

        // IsParsingNow updater
        this.WhenAnyObservable(x => x.StartParsingCommand.IsExecuting)
            .Subscribe(x=>IsParsingNow = x);
        
        #endregion

    }

    #endregion
    
    #region Commands
    
    /// <summary>
    ///     Open json file command
    /// </summary>
    public IReactiveCommand OpenFileCommand { get; }

    #region CancelParsingCommand :  Cancel StartParsingCommand

    /// <summary>
    ///     Cancel StartParsingCommand or OpenFileCommand
    /// </summary>
    public ReactiveCommand<Unit,Unit> CancelParsingCommand { get;  }
    
    private IObservable<bool> CanCancel => this.WhenAnyObservable(x => x.StartParsingCommand.IsExecuting, x => x.OpenFileCommand.IsExecuting,
        ((b, b1) => b1 || b));


    #endregion
    
    #region ClearDataCommand :  Clear data from Urls Store

    /// <summary>
    ///     Clear data from Urls Store
    /// </summary>
    public IReactiveCommand ClearDataCommand { get; }
    
    private IObservable<bool> CanClearData => 
        this.WhenAnyValue(
            x => x.IsParsingNow,
            x=>x.ServiceUrls,
            (isParsingNow,serviceUrls) 
                => !isParsingNow && serviceUrls.Any());


    #endregion
    
    #region StartParsingCommand :  Start parsing store service command

    /// <summary>
    ///     Start parsing store service command
    /// </summary>
    public IReactiveCommand StartParsingCommand { get;  }

    private IObservable<bool> CanStartParsing => 
        this.WhenAnyValue(
            x => x.ServiceUrls,
            x=>x.SelectedHtmlTag,
            (urls,tag) 
                => urls.Count != 0 && SelectedHtmlTag is not null );

    #endregion
    
    #endregion
    
}