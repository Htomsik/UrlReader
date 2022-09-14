namespace Core.Services.StatisticService;

/// <summary>
///     Statistic for urls collections
/// </summary>
public interface IUrlsStatisticService
{
    #region Collection

    public int UrlsCount { get; }
    
    public int UrlsAliveCount { get; }
    
    public int UrlsNotAliveCount { get; }
    
    public int UrlsUnknownCount { get; }

    #endregion


    #region Tags

    public int TagsCount { get; }
    
    public int TagsAverageCount { get; }
    
    public int TagsMaxValue { get; }
    
    public int TagsWithMaxValue { get; }
    
    #endregion
  
}