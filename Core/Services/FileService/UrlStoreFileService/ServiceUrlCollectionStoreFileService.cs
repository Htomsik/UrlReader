using System.Collections.ObjectModel;
using Core.Models;
using Core.Stores;
using Microsoft.Extensions.Logging;

namespace Core.Services.FileService.UrlStoreFileService;

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
    
   