using System;
using System.Collections.ObjectModel;
using AppInfrastructure.Stores.Repositories.Collection;
using Core.Models;
using DynamicData;

namespace Core.Stores;

/// <summary>
///     Store for ServiceUrl
/// </summary>
public sealed class ServiceUrlStore : BaseLazyCollectionRepository<ObservableCollection<ServiceUrl>,ServiceUrl>
{
    private IDisposable _changed;
    public override ObservableCollection<ServiceUrl>? CurrentValue
    {
        get => (ObservableCollection<ServiceUrl>?)_currentValue.Value;
        set
        {
            _currentValue = new Lazy<object?>((() => new ObservableCollection<ServiceUrl>(value)));

            if (value == null)
            {
                OnCurrentValueDeleted();
            }
            
            _changed?.Dispose();
            
            if (value != null)
            {
                _changed =  CurrentValue
                    .AsObservableChangeSet()
                    .WhenValueChanged(x => x.TagsCount)
                    .Subscribe(_ => OnCurrentValueChanged());
            }
            
            
            OnCurrentValueChanged();
        }
    }

    public ServiceUrlStore() => CurrentValue = new ObservableCollection<ServiceUrl>();
    
}
