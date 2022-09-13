using System.Collections.Generic;
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
public abstract  class BaseCollectionStoreTagParser<TCollection,TValue> : IStoreParser<string>
where TCollection : ICollection<TValue>
where TValue: ServiceUrl
{

    #region Stores and Serices
    
    private readonly IParser<ICollection<string>, string> _tagParser;

    #endregion
    
    #region Properties and Fields

    private  HttpClient _httpClient;

    private TCollection _urlsCollection;

    private readonly ILogger _logger;

    private readonly HtmlParser _htmlParser = new ();

    #endregion
    
    #region Constructors

    public BaseCollectionStoreTagParser(
        IStore<TCollection> urlsCollectionStore,
        IStore<HttpClient> httpClientStore,
        IParser<ICollection<string>,string> tagParser,
        ILogger<BaseCollectionStoreTagParser<TCollection,TValue>> logger)
    {
        #region Stores ans Services Initialize
        
        _tagParser = tagParser;

        #endregion

        #region Properties and Fields Initializing

        _httpClient = httpClientStore.CurrentValue;

        _urlsCollection = urlsCollectionStore.CurrentValue;

        _logger = logger;

        #endregion

        #region Sibsccriptions

        urlsCollectionStore.CurrentValueChangedNotifier += () => _urlsCollection = urlsCollectionStore.CurrentValue;

        httpClientStore.CurrentValueChangedNotifier += () => _httpClient = httpClientStore.CurrentValue;    

        #endregion

    }

    #endregion

    #region Methods

    #region Parse : Main parsing method

    public async Task Parse(string parameter,CancellationToken? cancelToken = null)
    {
        _logger.LogInformation("Starting collection parse...");

        var urlsArray = _urlsCollection.ToArray();
        
        for (int i = 0; i < urlsArray.Length; i++)
        {
            _logger.LogInformation("Parse {0} item",i);
           
            if (!await urlsArray[i].IsAliveAsync(_httpClient).ConfigureAwait(false))
            {
                urlsArray[i].State = UrlState.NotAlive;
               
                _logger.LogInformation("item {0} is not alive",i);
               
                continue;
            }
           
            await  IsAliveParse(urlsArray[i],parameter,cancelToken);
        }
        
    }

    #endregion
    
    #region IsAliveParse :  Additional method for Parse

    /// <summary>
    ///     Additional method for Parse
    /// </summary>
    /// <param name="value">Parsing value</param>
    /// <param name="parameter">Parsing parameter</param>
    /// <param name="cancelToken">Cancelation operation token</param>
    private async Task IsAliveParse(TValue value,string parameter,CancellationToken? cancelToken = null)
    {
        value.State = UrlState.Alive;
        
        _logger.LogInformation("Download html page");
        
        var stringHtml = await value.HtmlDownloadAsync(_httpClient);

        if (string.IsNullOrEmpty(stringHtml))
        {
            value.State = UrlState.Unknown;
            _logger.LogInformation("Can't download page");
            return;
        }
        
        var htmlDocument =  _htmlParser.ParseDocument(stringHtml);

        var tags = _tagParser.Parse(htmlDocument, parameter);

        value.TagsCount = tags.Count;
    }


    #endregion
  
    
    #endregion
    
}