using System;
using System.Collections.ObjectModel;
using AppInfrastructure.Services.NavigationServices.Close;
using AppInfrastructure.Stores.Repositories.Collection;
using DynamicData;
using UrlReader.Core.Models.Url;

namespace UrlReader.Core.Stores;

/// <summary>
///     Store for ServiceUrl
/// </summary>
public sealed class ServiceUrlStore : BaseLazyCollectionRepository<ObservableCollection<ServiceUrl>,ServiceUrl>, ICloseServices
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


    public async void Close() => CurrentValue = new();

    public ServiceUrlStore() => CurrentValue = new ObservableCollection<ServiceUrl>();
    
}
