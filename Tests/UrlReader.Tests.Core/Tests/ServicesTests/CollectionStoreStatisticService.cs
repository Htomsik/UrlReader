using System.Collections.ObjectModel;
using UrlReader.Core.Models;
using UrlReader.Core.Models.Url;
using UrlReader.Core.Services.StatisticService.Base;
using UrlReader.Core.Stores;

namespace UrlReader.Tests.Core.Tests.ServicesTests;

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
        
       
        Thread.Sleep(500); //Wait update
        
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