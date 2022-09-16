using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using AppInfrastructure.Stores.Repositories.Collection;
using Core.Models;
using DynamicData.Binding;

namespace Core.Stores;

/// <summary>
///     Store for ServiceUrl
/// </summary>
public sealed class ServiceUrlStore : BaseLazyCollectionRepository<ObservableCollection<ServiceUrl>,ServiceUrl>
{
    public ServiceUrlStore() => CurrentValue = new ObservableCollection<ServiceUrl>();

}