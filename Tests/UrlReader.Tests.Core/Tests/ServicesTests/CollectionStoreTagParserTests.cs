using System.Collections.ObjectModel;
using System.Net;
using Microsoft.Extensions.Logging;
using Moq;
using UrlReader.Core.Models.Url;
using UrlReader.Core.Services.ParserService.Base;
using UrlReader.Core.Services.ParserService.UrlStoreParser.Base;
using UrlReader.Core.Stores;
using UrlReader.Tests.Core.Resources;

namespace UrlReader.Tests.Core.Tests.ServicesTests;

[TestClass]
public class CollectionStoreTagParserTests
{
    /// <summary>
    ///     Check is all parsing system working
    /// </summary>
    [TestMethod]
    public async void IsParsing()
    {
        //Act
        var serviceUrls = new ServiceUrlStore();
        
        serviceUrls.AddIntoEnumerable(new ServiceUrl{Path = GlobalConstants.RightUrl.Path});
        
        var httpClient = HttpMocks.CreateHttpClient(HttpStatusCode.OK,GlobalConstants.HtmldocSampleString);

        var httpClientStore = new Mock<HttpClientStore>();

        httpClientStore.Setup(x => x.CurrentValue).Returns(httpClient);
        
        var tagParser = new TagParser();
        
        var mockLogger = new Mock<ILogger<BaseCollectionStoreTagParser<ObservableCollection<ServiceUrl>,ServiceUrl>>>();
        
        var storeTagParser = new BaseCollectionStoreTagParser<ObservableCollection<ServiceUrl>, ServiceUrl>(serviceUrls,httpClientStore.Object,tagParser,mockLogger.Object);
        
        //Arrange
        await storeTagParser.Parse("p", new CancellationToken());
       
        //Assert
        Assert.AreEqual(4,serviceUrls.CurrentValue.First().TagsCount);
    }
}