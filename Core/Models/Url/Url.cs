using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.Models;

public class Url : ReactiveObject
{
    /// <summary>
    ///     Path to site
    /// </summary>
    [Reactive]
    public string Path { get; set; }
}