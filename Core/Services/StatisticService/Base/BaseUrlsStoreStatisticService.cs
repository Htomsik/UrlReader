using System;
using System.Collections.Generic;
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
            
            ServiceUrls = urlsStore.CurrentValue;
            
            //Update Info
            ServiceUrls
                .ToObservableChangeSet()
                .AutoRefresh()
                .Subscribe(_ =>Update());
            
        };
        
        #endregion
        
        Update();
    }

    #endregion
    
    #region Methods

    /// <summary>
    ///     Update values
    /// </summary>
    private void Update()
    {
        var serviceUrls = ServiceUrls.ToList();

        UrlsCount = serviceUrls.Count;
            
        UrlsAliveCount = serviceUrls.Count(x => x.State == UrlState.Alive);
        UrlsNotAliveCount = serviceUrls.Count(x => x.State == UrlState.NotAlive);
        UrlsUnknownCount = serviceUrls.Count(x => x.State == UrlState.Unknown);

        TagsCount = serviceUrls.Sum(x=>x.TagsCount);
        TagsAverageCount = (int)(serviceUrls.Count != 0 ? serviceUrls.Average(x => x.TagsCount) : 0);
        TagsMaxValue = serviceUrls.Count != 0 ?  serviceUrls.Max(x=>x.TagsCount) : 0;
        TagsWithMaxValue = serviceUrls.Count(x=>x.IsMaxValue);

        foreach (var item in serviceUrls)
        {
            item.IsMaxValue =  
                TagsMaxValue != 0 
                && item.TagsCount != 0  
                && TagsMaxValue == item.TagsCount;
        }
    }

    #endregion
}