using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using UrlReader.Core.Models.Url;

namespace UrlReader.Tests.Core.Resources;

internal static class GlobalConstants
{
    #region Url

    public static readonly ServiceUrl RightUrl = new ServiceUrl{Path = "https://SomeSite.com" };
    
    public static readonly ServiceUrl ErrorUrl = new ServiceUrl{Path = "NoSite" };
    
    public static readonly ServiceUrl NullPathUrl = new ServiceUrl{Path = "            " };
    
    public static readonly ServiceUrl NullUrl = null;

    #endregion
    
    #region json


    public static string RightUrlJson() => "{\"path\": \"https://SomeSite.com\"}" ;


    public static string ErrorUrlJson() => "{\"path\": \"NoSite\"}";
    
    public static string NullPathUrlJson() =>"{\"path\": \" \"}";

    public static string NullUrlJson() => "[]";
    
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