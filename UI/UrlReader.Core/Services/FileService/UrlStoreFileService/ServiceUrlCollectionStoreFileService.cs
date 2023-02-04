using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using UrlReader.Core.Models.Url;
using UrlReader.Core.Services.FileService.Base;
using UrlReader.Core.Services.FileService.UrlStoreFileService.Base;
using UrlReader.Core.Stores;

namespace UrlReader.Core.Services.FileService.UrlStoreFileService;

/// <summary>
///     IStoreFileService realization for ServiceUrlStore
/// </summary>
public sealed class ServiceUrlCollectionStoreFileService : BaseJsonCollectionStoreFileService<ObservableCollection<ServiceUrl>,ServiceUrl>
{
    public ServiceUrlCollectionStoreFileService(
        ServiceUrlStore store,
        JsonClientFileService jsonFileService, 
        ILogger<BaseJsonCollectionStoreFileService<ObservableCollection<ServiceUrl>,ServiceUrl>> logger) : base(store, jsonFileService, logger)
    {
    }
}
    
   