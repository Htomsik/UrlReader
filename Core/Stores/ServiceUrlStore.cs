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
    public override ObservableCollection<ServiceUrl>? CurrentValue
    {
        get => (ObservableCollection<ServiceUrl>?)_currentValue.Value;
        set
        {
            _currentValue =  new Lazy<object?>(() => value);
            
            if(value is null || value.Equals(default))
                OnCurrentValueDeleted();
            
            CurrentValue
                ?.WhenValueChanged(x => x.Count)
                .Subscribe(_ => OnCurrentValueChanged());
            
            BindingOperations.EnableCollectionSynchronization(CurrentValue , _lock);
            
            OnCurrentValueChanged();
            
        }
        
    }
    
    private static object _lock = new object();
    
    public ServiceUrlStore() => CurrentValue = new ObservableCollection<ServiceUrl>();

}