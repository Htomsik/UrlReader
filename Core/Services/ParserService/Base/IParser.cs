using AngleSharp.Html.Dom;

namespace Core.Services.ParserService;

/// <summary>
///     Parser service
/// </summary>
public interface IParser<TOutput,TInput>
{
    /// <summary>
    ///     Default parser
    /// </summary>
    /// <param name="htmlDocument">Html document</param>
    /// <param name="parameter">Input parameter</param>
    /// <returns>TOutput parameter</returns>
    TOutput Parse(IHtmlDocument htmlDocument,TInput parameter);
}