using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;

namespace Core.Services.ParserService;

/// <summary>
///     Tag Parser
/// </summary>
public class TagParser : IParser<ICollection<string>,string>
{
 
    /// <summary>
    ///     Parse all tags from html page
    /// </summary>
    /// <param name="htmlDocument">htmlDocument type</param>
    /// <param name="parameter">tag</param>
    /// <returns>list with found tags</returns>
    /// <exception cref="ArgumentNullException">If htmlDocument if parameter is null</exception>
    public  ICollection<string> Parse(IHtmlDocument htmlDocument, string parameter)
    {
        if (htmlDocument is null)
            throw new ArgumentNullException(nameof(htmlDocument));
        
        if (string.IsNullOrEmpty(parameter))
            throw new ArgumentNullException(nameof(parameter));
       
        
        ICollection<string> collection = new List<string>();

        var items = htmlDocument.QuerySelectorAll(parameter).Where(item => item.TagName != null);

        foreach(var item in items)
        {
            collection.Add(item.TextContent);
        }

        return collection;
    }
}
  