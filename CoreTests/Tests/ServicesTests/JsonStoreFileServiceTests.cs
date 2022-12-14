using System.Collections.ObjectModel;
using AppInfrastructure.Stores.DefaultStore;
using Core.Models;
using Core.Services.FileService.UrlStoreFileService;
using CoreTests.Resources;
using Microsoft.Extensions.Logging;
using Moq;

namespace CoreTests.Tests.ServicesTests;

[TestClass]
public class JsonStoreFileServiceTests
{
    /// <summary>
    ///     Is BaseJsonStoreFileService convert and place into store
    /// </summary>
    [TestMethod]
    public void IsJsonConvertedWhenAllRight()
    {
        //Act
        var serviceStore = new BaseLazyStore<ObservableCollection<ServiceUrl>>();

        serviceStore.CurrentValue = new ObservableCollection<ServiceUrl>();
        
        var mockLogger = new Mock<ILogger<BaseJsonCollectionStoreFileService<ObservableCollection<ServiceUrl>,ServiceUrl>>>();
        
        var storeJsonConverter = new BaseJsonCollectionStoreFileService<ObservableCollection<ServiceUrl>,ServiceUrl>(serviceStore,ServicesMocks.CreateFileService(GlobalConstants.RightUrlJson()),mockLogger.Object);
        
        //Assert
        storeJsonConverter.GetDataFromFile(new CancellationToken()).Wait();
        
        //Arrange
        Assert.AreEqual(GlobalConstants.RightUrl.Path,serviceStore.CurrentValue.First().Path);
    }
    
    /// <summary>
    ///     Is BaseJsonStoreFileService not place into store when value is not right
    /// </summary>
    [TestMethod]
    public void IsJsonNotConvertedWhenDataInOtherType()
    {
        //Act
        var serviceStore = new BaseLazyStore<ObservableCollection<ServiceUrl>>();
        
        var mockLogger = new Mock<ILogger<BaseJsonCollectionStoreFileService<ObservableCollection<ServiceUrl>,ServiceUrl>>>();
        
        var storeJsonConverter = new BaseJsonCollectionStoreFileService<ObservableCollection<ServiceUrl>,ServiceUrl>(serviceStore,ServicesMocks.CreateFileService("NotAUrl"),mockLogger.Object);
        
        //Assert
        storeJsonConverter.GetDataFromFile(new CancellationToken()).Wait();
        
        //Arrange
        Assert.IsNull(serviceStore.CurrentValue);
    }
    
    
  
}