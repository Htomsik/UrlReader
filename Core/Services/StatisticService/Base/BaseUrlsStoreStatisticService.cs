using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AppInfrastructure.Stores.DefaultStore;
using Core.Models;
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
    
    private int _oldMaxValue = 0;
    
    #endregion

    #region Constructors

    public BaseUrlsStoreStatisticService(IStore<ObservableCollection<ServiceUrl>> urlsStore)
    {
        #region Properties and Fields Initializing

        ServiceUrls = urlsStore.CurrentValue;

        #endregion

        #region Subscriptions

        urlsStore.CurrentValueChangedNotifier += () =>
        {
            if (ServiceUrls.Count != urlsStore.CurrentValue.Count )
                Close();
            
            ServiceUrls = urlsStore.CurrentValue;
        
            //Spesial for update statistic
            this.RaisePropertyChanged(nameof(ServiceUrls));
            
        };
        
        //Update statistic. 200 ms is timer for wait non used momet of urlsStore into oter flow
        this.WhenPropertyChanged(x=>x.ServiceUrls)
            .Throttle(TimeSpan.FromMilliseconds(1000))
            .Subscribe(_ => Update());
        
        #endregion
        
    }

    #endregion
    
    #region Methods

    #region Update : Update statistic

    /// <summary>
    ///     Update values
    /// </summary>
    public async void Update()
    {
       await  Task.Run(() =>
        {
            UrlsCount = ServiceUrls.Count;

            UrlsAliveCount = ServiceStateCount(UrlState.Alive);
            UrlsNotAliveCount = ServiceStateCount(UrlState.NotAlive);
            UrlsUnknownCount = ServiceStateCount(UrlState.Unknown);

            TagsCount = ServiceUrls.Sum(x => x.TagsCount);

            TagsAverageCount = (int)(ServiceStateCount(UrlState.Alive) != 0
                ? ServiceUrls.Where(x => x.State == UrlState.Alive).Average(x => x.TagsCount)
                : 0);

            TagsMaxValue = ServiceUrls.Count != 0 ? ServiceUrls.Max(x => x.TagsCount) : 0;

            TagsWithMaxValue = ServiceCount(x => x.TagsCount == TagsMaxValue);

            if (_oldMaxValue == TagsMaxValue && TagsWithMaxValue == ServiceUrls.Count(x => x.IsMaxValue))
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

        });

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

    public void Close()
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


