using System.Net;
using UrlReader.Core.Extensions;
using UrlReader.Tests.Core.Resources;

namespace UrlReader.Tests.Core.Tests.ExtensionsTests.UrlExtensions;

[TestClass]
public class HtmlDownloadAsyncTests
{
    /// <summary>
    ///     Is HtmlDownloadAsync return downloaded string
    /// </summary>
    [TestMethod]
    public void IsDownloading()
    {
        //Arrange
        var downloadedString = "Downloaded";
        
        var httpClient = HttpMocks.CreateHttpClient(HttpStatusCode.OK,downloadedString);
        
        //Act+Assert
        Assert.AreEqual(downloadedString,GlobalConstants.RightUrl.HtmlDownloadAsync(new CancellationToken(),httpClient).Result);
    }
    
    
    /// <summary>
    ///     Is HtmlDownloadAsync return null when Url is not right
    /// </summary>
    [TestMethod]
    public void IsNullWhenUrlIsNotRight()
    {
        //Arrange
        var downloadedString = "Downloaded";
        
        var httpClient = HttpMocks.CreateHttpClient(HttpStatusCode.OK,downloadedString);
        
        //Act+Assert
        Assert.IsNull(GlobalConstants.ErrorUrl.HtmlDownloadAsync(new CancellationToken(),httpClient).Result);
    }
    
     
    /// <summary>
    ///     Is HtmlDownloadAsync return null when Response timeout ends
    /// </summary>
    [TestMethod]
    public void IsNullWhenTimeOut()
    {
        //Arrange
        var downloadedString = "Downloaded";
        
        var httpClient = HttpMocks.CreateHttpClient(HttpStatusCode.OK,downloadedString,true);
        
        //Act+Assert
        Assert.IsNull(GlobalConstants.RightUrl.HtmlDownloadAsync(new CancellationToken(),httpClient).Result);
    }
    
    
    /// <summary>
    ///     Is HtmlDownloadAsync throw exception when Url or Url.path is null
    /// </summary>
    [TestMethod]
    public void IsThrowExceptionIfUrlOrPathEmpty()
    {
        //Arrange
        var httpClient = HttpMocks.CreateHttpClient(HttpStatusCode.OK);

        //Act+Assert
        Assert.ThrowsException<AggregateException>(() =>
            GlobalConstants.NullPathUrl.HtmlDownloadAsync(new CancellationToken(), httpClient).Result);
        Assert.ThrowsException<AggregateException>(() =>
            GlobalConstants.NullUrl.HtmlDownloadAsync(new CancellationToken(), httpClient).Result);
    }
    
    
   
}