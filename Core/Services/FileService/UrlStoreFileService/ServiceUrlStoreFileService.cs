using System.Collections.ObjectModel;
using Core.Models;
using Core.Stores;
using Microsoft.Extensions.Logging;

namespace Core.Services.FileService.UrlStoreFileService;

/// <summary>
///     IStoreFileService realization for ServiceUrlStore
/// </summary>
public sealed class ServiceUrlStoreFileService : BaseJsonStoreFileService<ObservableCollection<ServiceUrl>>
{
    public ServiceUrlStoreFileService(
        ServiceUrlStore store,
        JsonClientFileService jsonFileService, 
        ILogger<BaseJsonStoreFileService<ObservableCollection<ServiceUrl>>> logger) : base(store, jsonFileService, logger)
    {
    }
}
    
   