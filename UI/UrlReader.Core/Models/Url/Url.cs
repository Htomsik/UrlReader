using System.Text.Json.Serialization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace UrlReader.Core.Models.Url;

/// <summary>
///     Url of site
/// </summary>
public class Url : ReactiveObject
{
    /// <summary>
    ///     Path to site
    /// </summary>
    [Reactive] [JsonInclude] public string Path { get; set; }
}