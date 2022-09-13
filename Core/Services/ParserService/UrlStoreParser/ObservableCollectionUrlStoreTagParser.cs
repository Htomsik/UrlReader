using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using AppInfrastructure.Stores.DefaultStore;
using Core.Models;
using Core.Stores;
using Microsoft.Extensions.Logging;

namespace Core.Services.ParserService.UrlStoreParser;

/// <summary>
///         Tag parser for Istore with ObservableCollection<ServiceUrl>
/// </summary>
public sealed class ObservableCollectionUrlStoreTagParser : BaseCollectionStoreTagParser<ObservableCollection<ServiceUrl>,ServiceUrl> 
{
    public ObservableCollectionUrlStoreTagParser(
        ServiceUrlStore urlsCollectionStore,
        HttpClientStore httpClientStore, 
        TagParser tagParser, 
        ILogger<BaseCollectionStoreTagParser<ObservableCollection<ServiceUrl>, ServiceUrl>> logger) 
        : base(urlsCollectionStore, httpClientStore, tagParser, logger)
    {
    }
}