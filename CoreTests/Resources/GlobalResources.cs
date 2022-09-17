using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Core.Models;


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