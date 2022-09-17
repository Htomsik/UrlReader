using System.Collections.ObjectModel;
using Core.Models;
using Core.Services.StatisticService;
using Core.Stores;

namespace CoreTests.Tests.ServicesTests;

[TestClass]
public class CollectionStoreStatisticServiceTest
{
    /// <summary>
    ///    Check all statistics for correct values
    /// </summary>
    [TestMethod]
    public void isRightStatistic()
    {
        //Act
        var serviceUrls = new ServiceUrlStore();

        var urlWithMAxTags = new ServiceUrl { TagsCount = 5 };
        
        var someurl = new ServiceUrl { TagsCount = 4 };

        serviceUrls.CurrentValue = new ObservableCollection<ServiceUrl>
        {
            urlWithMAxTags,
            urlWithMAxTags,
            someurl,
        };

        var Stats = new BaseUrlsStoreStatisticService(serviceUrls);

        //Arrange
        serviceUrls.RemoveFromEnumerable(someurl);
        
        Stats.Update();
        
        //Assert
        Assert.AreEqual(Stats.TagsCount,serviceUrls.CurrentValue.Sum(x=>x.TagsCount));
        
        Assert.AreEqual(Stats.TagsMaxValue,serviceUrls.CurrentValue.Max(x=>x.TagsCount));
        
        Assert.AreEqual(Stats.TagsWithMaxValue,serviceUrls.CurrentValue.Count(x=>x.TagsCount == urlWithMAxTags.TagsCount));
        
        Assert.AreEqual(Stats.TagsAverageCount,serviceUrls.CurrentValue.Count(x=>x.State == UrlState.Alive) != 0 ? serviceUrls.CurrentValue.Where(x=>x.State == UrlState.Alive).Average(x=>x.TagsCount) : 0 );
        
        Assert.AreEqual(Stats.UrlsCount,serviceUrls.CurrentValue.Count);
        
        Assert.AreEqual(Stats.UrlsAliveCount,serviceUrls.CurrentValue.Count(x=>x.State == UrlState.Alive));
        
        Assert.AreEqual(Stats.UrlsNotAliveCount,serviceUrls.CurrentValue.Count(x=>x.State == UrlState.NotAlive));
        
        Assert.AreEqual(Stats.UrlsUnknownCount,serviceUrls.CurrentValue.Count(x=>x.State == UrlState.Unknown));
    }
}