using AppInfrastructure.Services.NavigationServices.Close;

namespace UrlReader.Core.Services.StatisticService.Base;

/// <summary>
///     Statistic for urls collections
/// </summary>
public interface IUrlsStatisticService : ICloseServices
{
    #region Collection

    /// <summary>
    ///     Urls counts in collection
    /// </summary>
    public int UrlsCount { get; }
    
    /// <summary>
    ///      Urls counts with Alive state
    /// </summary>
    public int UrlsAliveCount { get; }
    
    /// <summary>
    ///      Urls counts with NotAlive state
    /// </summary>
    public int UrlsNotAliveCount { get; }
    
    
    /// <summary>
    ///      Urls counts with Unknown state
    /// </summary>
    public int UrlsUnknownCount { get; }

    #endregion
    
    #region Tags
    
    /// <summary>
    ///     Tags counts in collection
    /// </summary>
    public int TagsCount { get; }
    
    /// <summary>
    ///     Average tags counts
    /// </summary>
    public int TagsAverageCount { get; }
    
    /// <summary>
    ///     MaxValue of tags
    /// </summary>
    public int TagsMaxValue { get; }
    
    
    /// <summary>
    ///     Count of tags with MaxValue
    /// </summary>
    public int TagsWithMaxValue { get; }
    
    #endregion
    
}