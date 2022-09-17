using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Core.Models;
using Newtonsoft.Json;

namespace CoreTests.Resources;

internal static class GlobalConstants
{
    #region Url

    public static readonly Url RightUrl = new Url{Path = "https://SomeSite.com" };
    
    public static readonly Url ErrorUrl = new Url{Path = "NoSite" };
    
    public static readonly Url NullPathUrl = new Url{Path = "            " };
    
    public static readonly Url NullUrl = null;

    #endregion
    
    #region json


    public static string JsFormater<T>(T value) => JsonConvert.SerializeObject(value, Formatting.Indented);
    
    public static string RightUrlJson() => JsFormater(RightUrl);
    
    public static string ErrorUrlJson() => JsFormater(ErrorUrl);
    
    public static string NullPathUrlJson() => JsFormater(NullPathUrl);

    public static string NullUrlJson() => JsFormater(NullUrl);
    
    #endregion

    #region Html

    public static readonly string HtmldocSampleString = 
        "<html><head><title>Пример веб-страницы</title></head><body><p>Первый абзац.</p><p>Второй абзац.</p><p>Первый абзац.</p><p>Второй абзац.</p></body></html>";

    public static IHtmlDocument HtmlDocumentSample()
    {
        HtmlParser parser = new();

        return parser.ParseDocument(HtmldocSampleString);
    }

   

    #endregion
    
 

}