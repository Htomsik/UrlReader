using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.Models;

/// <summary>
///     Url of site
/// </summary>
public class Url : ReactiveObject
{
    /// <summary>
    ///     Path to site
    /// </summary>
    [Reactive] [JsonProperty] public string Path { get; set; }
}