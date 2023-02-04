using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using UrlReader.Core.Models.Url;
using UrlReader.Core.Services.ParserService.Base;
using UrlReader.Core.Services.ParserService.UrlStoreParser.Base;
using UrlReader.Core.Stores;

namespace UrlReader.Core.Services.ParserService.UrlStoreParser;

/// <summary>
///         Tag parser for IStore with ObservableCollection<ServiceUrl>
/// </summary>
public sealed class ServiceUrlStoreTagParser : BaseCollectionStoreTagParser<ObservableCollection<ServiceUrl>,ServiceUrl> 
{
    public ServiceUrlStoreTagParser(
        ServiceUrlStore urlsCollectionStore,
        HttpClientStore httpClientStore, 
        TagParser tagParser, 
        ILogger<BaseCollectionStoreTagParser<ObservableCollection<ServiceUrl>, ServiceUrl>> logger) 
        : base(urlsCollectionStore, httpClientStore, tagParser, logger)
    {
    }
}