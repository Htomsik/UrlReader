using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using AppInfrastructure.Stores.DefaultStore;
using Core.Extensions;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Core.Services.ParserService.UrlStoreParser;

/// <summary>
///     Realization of IStoreParser for parsing tags Urls into colletions
/// </summary>
/// <typeparam name="TCollection">Some collection woth ServiceUrls</typeparam>
/// <typeparam name="TValue">ServiceUrl</typeparam>
public class BaseCollectionStoreTagParser<TCollection, TValue> : IStoreParser<string>
    where TCollection : ICollection<TValue>
    where TValue : ServiceUrl
{
    #region Stores and Serices

    private readonly IParser<ICollection<string>, string> _tagParser;

    #endregion

    #region Constructors

    public BaseCollectionStoreTagParser(
        IStore<TCollection> urlsCollectionStore,
        IStore<HttpClient> httpClientStore,
        IParser<ICollection<string>, string> tagParser,
        ILogger<BaseCollectionStoreTagParser<TCollection, TValue>> logger)
    {
        #region Stores ans Services Initialize

        _tagParser = tagParser;

        #endregion

        #region Properties and Fields Initializing

        _httpClient = httpClientStore.CurrentValue;

        _urlsCollection = urlsCollectionStore.CurrentValue;

        _logger = logger;

        #endregion

        #region Subscriptions

        urlsCollectionStore.CurrentValueChangedNotifier += () => _urlsCollection = urlsCollectionStore.CurrentValue;

        httpClientStore.CurrentValueChangedNotifier += () => _httpClient = httpClientStore.CurrentValue;

        #endregion
    }

    #endregion

    #region Properties and Fields

    private HttpClient _httpClient;

    private TCollection _urlsCollection;

    private readonly ILogger _logger;

    private readonly HtmlParser _htmlParser = new();

    #endregion

    #region Methods

    #region Parse : Main parsing method

    public async Task Parse(string parameter, CancellationToken cancelToken)
    {
        _logger.LogInformation("Starting collection parse...");

        // id of current processing part
        var partIdCounter = 0;
        
        // Parts of main collection
        var parts = _urlsCollection.Partition(100).ToList();
        
        // Operation time counter
        var loggerTimer = Stopwatch.StartNew();
            
        foreach (var pack in parts)
        {
            _logger.LogInformation("Starting {0}/{1} part of collection",++partIdCounter,parts.Count);
                
            await PartCollectionParsing(pack,partIdCounter,parts.Count, parameter, cancelToken);
        }
        
        _logger.LogInformation("End collection parsing. Processed {0}/{1}. Opearion time:{2}",_urlsCollection.Count(x=>x.State != UrlState.Unknown),_urlsCollection.Count,loggerTimer.Elapsed.TotalSeconds);
        
        loggerTimer.Stop();
    }

    #endregion
    
    #region PartCollectionParsing :  Parse collection

    /// <summary>
    ///     Parse of parts
    /// </summary>
    /// <param name="partOfMain">Part of main collection</param>
    /// <param name="partId">Part Id into main collection</param>
    /// <param name="partsCount">Count of all parts</param>
    /// <param name="parameter">Parsing parameter</param>
    /// <param name="cancelToken">Cancel operation token</param>
    async Task PartCollectionParsing(IEnumerable<TValue> partOfMain,int partId,int partsCount,string parameter, CancellationToken cancelToken)
    {
        // old values counter of Urls with Unknown state
        var oldUnknownCount = 0;
        
        // Counter of current count
        var cycleCount = 1;
        
        do
        {
            var tasks = new List<Task>();

            var unknowns = partOfMain.Where(x => x.State == UrlState.Unknown).ToArray();
            
            await foreach (var elem in unknowns.AsAsync().WithCancellation(cancelToken))
            {
                var newTask = Task.Run(async () =>
                {
                    try
                    {
                        elem.State = await elem.CheckState(cancelToken, _httpClient);

                        if (elem.State is UrlState.Alive)
                            await IsAliveParse(elem, parameter, cancelToken);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e,"Failed parse item:{0}/{1}",partId,partsCount);
                    }
                    
                },cancelToken);
            
                tasks.Add(newTask);
                
            }
            
            await Task.Run(() =>
            {
                _logger.LogInformation("Parsing... part:{0}/{1} cycle: {2}/5",partId,partsCount,cycleCount);
                Task.WaitAll(tasks.ToArray());
            },cancelToken).ConfigureAwait(false);

            if (oldUnknownCount == unknowns.Length)
                break;
            
            oldUnknownCount = unknowns.Length;

        } while (++cycleCount <= 5);

    }

    #endregion
    
    #region IsAliveParse :  Additional method for Parse

    /// <summary>
    ///     Additional method for Parse
    /// </summary>
    /// <param name="url">Parsing value</param>
    /// <param name="parameter">Parsing parameter</param>
    /// <param name="cancelToken">Cancelation operation token</param>
    private async Task IsAliveParse(TValue url, string parameter, CancellationToken cancelToken)
    {
        
        var stringHtml = await url.HtmlDownloadAsync(cancelToken, _httpClient);

        if (string.IsNullOrEmpty(stringHtml))
        {
            url.State = UrlState.Unknown;
            return;
        }

        var htmlDocument = await _htmlParser.ParseDocumentAsync(stringHtml, cancelToken);

        var tags = _tagParser.Parse(htmlDocument, parameter);

        url.TagsCount = tags.Count;
    }

    #endregion
    
    #endregion
}