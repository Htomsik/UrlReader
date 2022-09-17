using System;
using System.Reactive.Linq;
using System.Text.Json.Serialization;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;

namespace Core.Models;

/// <summary>
///     Urls types for service usings
/// </summary>
public class ServiceUrl : Url
{
    #region Properties

    /// <summary>
    ///     count of tags
    /// </summary>
    [Reactive] [JsonIgnore] public int TagsCount { get; set; } = 0;
    
    /// <summary>
    ///     State of Url
    /// </summary>
    [Reactive] [JsonIgnore] public UrlState State { get; set; } = UrlState.Unknown;
    
    /// <summary>
    ///     True when any propertychanged
    /// </summary>
    [Reactive] [JsonIgnore]  public bool IsUsingNow { get; private set; } = false;
    
    /// <summary>
    ///     True if current value is Maximum of all URls in collection
    /// </summary>
    [Reactive] [JsonIgnore] public bool IsMaxValue { get; set; } = false;


    #endregion

    #region Constructors

    public ServiceUrl()
    {
        this.WhenPropertyChanged(x=>x.Path)
            .Subscribe(_=>IsUsingNow = true);
        
        this.WhenPropertyChanged(x=>x.TagsCount)
            .Subscribe(_=>IsUsingNow = true);
        
        this.WhenPropertyChanged(x=>x.State)
            .Subscribe(_=>IsUsingNow = true);
        
        this.WhenPropertyChanged(x=>x.IsMaxValue)
            .Subscribe(_=>IsUsingNow = true);
        
        this.WhenPropertyChanged(x => x.IsUsingNow)
            .Throttle(TimeSpan.FromSeconds(1))
            .Subscribe(_=>IsUsingNow = false);
    }


    #endregion

}