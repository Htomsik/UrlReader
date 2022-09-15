using Core.Models;

namespace CoreTests.Resources;

internal static class GlobalConstans
{
    public static readonly Url RightUrl = new Url{Path = "https://SomeSite.com" };
    
    public static readonly Url ErrorUrl = new Url{Path = "NoSite" };
    
    public static readonly Url NullPathUrl = new Url{Path = "            " };
    
    public static readonly Url NullUrl = null;
}