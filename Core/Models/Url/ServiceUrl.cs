using ReactiveUI.Fody.Helpers;

namespace Core.Models;

public class ServiceUrl : Url
{

    /// <summary>
    ///     count of <a> tags
    /// </summary>
    [Reactive]
    public int TagsCount { get; set; } = default;

    /// <summary>
    ///     State of Url
    /// </summary>
    [Reactive]
    public UrlState State { get; set; } = UrlState.Unknown;
}