using System.Collections.ObjectModel;
using AppInfrastructure.Stores.Repositories.Collection;
using Core.Models;

namespace Core.Stores;

/// <summary>
///     Store for ServiceUrl
/// </summary>
public sealed class ServiceUrlStore : BaseLazyCollectionRepository<ObservableCollection<ServiceUrl>,ServiceUrl>
{
    
}