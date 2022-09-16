using System;
using System.Collections.ObjectModel;
using System.Linq;
using AppInfrastructure.Stores.DefaultStore;
using Core.Models;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.Services.StatisticService;

/// <summary>
///     Static service for IUrlsStatisticService with store
/// </summary>
public class BaseUrlsStoreStatisticService : ReactiveObject,IUrlsStatisticService
{
    #region Properties

    #region Urls

    [Reactive] 
    public int UrlsCount { get; private set; }
    
    [Reactive]
    public int UrlsAliveCount { get; private set; } 
    
    [Reactive]
    public int UrlsNotAliveCount { get; private set; }
    
    [Reactive]
    public int UrlsUnknownCount { get; private set; }
    
    #endregion
    
    #region Tags

    [Reactive]
    public int TagsCount { get; private set; }
    
    [Reactive]
    public int TagsAverageCount { get; private set;}
    
    [Reactive]
    public int TagsMaxValue { get; private set; }
    
    [Reactive]
    public int TagsWithMaxValue { get; private set;}
    
    
    #endregion
    
    #endregion

    #region Fields
    
    [Reactive]
    private ObservableCollection<ServiceUrl> ServiceUrls { get; set; }

    private IDisposable _serviceUrlsUpdater;

    private int _oldMaxValue = 0;
    #endregion

    #region Constructors

    public BaseUrlsStoreStatisticService(IStore<ObservableCollection<ServiceUrl>> urlsStore)
    {
        #region Properties and Fields Initializing

        ServiceUrls = urlsStore.CurrentValue;

        #endregion

        #region Subscription

        urlsStore.CurrentValueChangedNotifier += () =>
        {

            if (ServiceUrls.Count != urlsStore.CurrentValue.Count )
                Clear();
            
            ServiceUrls = urlsStore.CurrentValue;
            
            _serviceUrlsUpdater?.Dispose();
            
            _serviceUrlsUpdater = ServiceUrls
                .ToObservableChangeSet()
                .AutoRefresh(TimeSpan.FromSeconds(5))
                .Subscribe(_ =>Update());
            
           
        };
        
        #endregion
        
    }

    #endregion
    
    #region Methods

    #region Update : Update statistic

    /// <summary>
    ///     Update values
    /// </summary>
    private void Update()
    {
        UrlsCount = ServiceUrls.Count;

        UrlsAliveCount = ServiceStateCount(UrlState.Alive);
        UrlsNotAliveCount = ServiceStateCount(UrlState.NotAlive);
        UrlsUnknownCount = ServiceStateCount(UrlState.Unknown);

        TagsCount = ServiceUrls.Sum(x => x.TagsCount);
        
        TagsAverageCount = (int)(ServiceStateCount(UrlState.Alive) != 0 ? ServiceUrls.Where(x=>x.State == UrlState.Alive).Average(x => x.TagsCount) : 0);
        
        TagsMaxValue = ServiceUrls.Count != 0 ? ServiceUrls.Max(x => x.TagsCount) : 0;
        
        TagsWithMaxValue = ServiceCount(x=>x.TagsCount == TagsMaxValue);
        
        if (_oldMaxValue == TagsMaxValue && TagsWithMaxValue == ServiceUrls.Count(x=>x.IsMaxValue))
            return;
        
        _oldMaxValue = TagsMaxValue;

        if (TagsMaxValue == 0)
            return;
        
        foreach (var serviceUrl in ServiceUrls.Where(x => x.IsMaxValue))
        {
            serviceUrl.IsMaxValue = false;
        }

        foreach (var serviceUrl in ServiceUrls.Where(x => x.TagsCount == TagsMaxValue))
        {
            serviceUrl.IsMaxValue = true;
        }

    }

    #endregion
    
    #region Addons 

    /// <summary>
    ///     For code readability
    /// </summary>
    private int ServiceCount(Func<ServiceUrl, bool> func) => ServiceUrls.Count(func);
    
    /// <summary>
    ///     For code readability
    /// </summary>
    private int ServiceStateCount(UrlState urlState) => ServiceCount(x=>x.State == urlState);

    private void Clear()
    {
        UrlsCount = 0;
        UrlsAliveCount = 0;
        UrlsNotAliveCount = 0;
        UrlsUnknownCount = 0;

        TagsCount = 0;
        TagsAverageCount = 0;
        TagsMaxValue = 0;
        TagsWithMaxValue = 0;
    }
    
    #endregion
    
    #endregion

}


