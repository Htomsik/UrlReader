using Newtonsoft.Json;
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
}