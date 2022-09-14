using System;
using System.Collections.Generic;
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
    public string? LastLog { get; private set; }
    
    [Reactive]
    public ObservableCollection<ServiceUrl> ServiceUrls { get; private set; }

    [Reactive] public string SelectedHtmlTag { get; set; } 
    
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

        SelectedHtmlTag = HtmlTagsList.First();
        
        #endregion
        
        #region Subscriptions

        serviceUrlStore.CurrentValueChangedNotifier += () => ServiceUrls = serviceUrlStore.CurrentValue;
        
        loggerStore.CurrentValueChangedNotifier += () => LastLog = loggerStore.CurrentValue.Last();

        // Will set LastLog to null after 6 seconds after changing
        this.WhenAnyValue(x => x.LastLog)
            .Throttle(TimeSpan.FromSeconds(6))
            .Subscribe(_ => LastLog = null);

        // Urls stats updater
        this.WhenAnyPropertyChanged()
            .Subscribe(_ =>
            {
                AlivesUrlsCount = ServiceUrls.Count(x => x.State == UrlState.Alive);
                NotAlivesUrlsCount = ServiceUrls.Count(x => x.State == UrlState.NotAlive);
                UnknownUrlsCount = ServiceUrls.Count(x => x.State == UrlState.Unknown);
            });

        // TagsCounter when change SelectedHtmlTag Updater
        this.WhenAnyValue(x => x.SelectedHtmlTag)
            .Subscribe(_ =>
            {
                foreach (var item in serviceUrlStore.CurrentValue)
                {
                    item.TagsCount = 0;
                }
            });
        
        #endregion

        #region Commands Initializing
        
        StartParsingCommand = ReactiveCommand.CreateFromObservable(
            ()=>
                Observable
                    .StartAsync(ct=>tagParser.Parse(SelectedHtmlTag,ct))
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
        this.WhenAnyValue(
            x => x.ServiceUrls,
            x=>x.SelectedHtmlTag,
            (urls,tag) 
                => urls.Count != 0 && SelectedHtmlTag is not null );

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