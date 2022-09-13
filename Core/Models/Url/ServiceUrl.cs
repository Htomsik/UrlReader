using System;
using System.Reactive.Linq;
using DynamicData.Binding;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.Models;

public class ServiceUrl : Url
{

    /// <summary>
    ///     count of <a> tags
    /// </summary>
    [Reactive]
    [JsonIgnore]
    public int TagsCount { get; set; } = default;

    /// <summary>
    ///     State of Url
    /// </summary>
    [Reactive]
    [JsonIgnore]
    public UrlState State { get; set; } = UrlState.Unknown;


    [JsonIgnore]
    [Reactive]
    public bool IsParsingNow { get; private set; }

    public ServiceUrl()
    {
        this.WhenPropertyChanged(x=>x.Path)
            .Subscribe(_=>IsParsingNow = true);
        
        this.WhenPropertyChanged(x=>x.TagsCount)
            .Subscribe(_=>IsParsingNow = true);
        
        this.WhenPropertyChanged(x=>x.State)
            .Subscribe(_=>IsParsingNow = true);

        this.WhenPropertyChanged(x => x.IsParsingNow)
            .Throttle(TimeSpan.FromSeconds(1))
            .Subscribe(_=>IsParsingNow = false);
    }
    
}