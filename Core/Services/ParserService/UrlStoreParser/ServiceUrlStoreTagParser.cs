using System.Collections.ObjectModel;
using Core.Models;
using Core.Stores;
using Microsoft.Extensions.Logging;

namespace Core.Services.ParserService.UrlStoreParser;

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