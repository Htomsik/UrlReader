using System.Net;
using Core.Extensions;
using CoreTests.Resources;

namespace CoreTests.ExtensionsTests.UrlExtensions;

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
        Assert.AreEqual(downloadedString,GlobalConstans.RightUrl.HtmlDownloadAsync(new CancellationToken(),httpClient).Result);
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
        Assert.IsNull(GlobalConstans.ErrorUrl.HtmlDownloadAsync(new CancellationToken(),httpClient).Result);
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
        Assert.IsNull(GlobalConstans.RightUrl.HtmlDownloadAsync(new CancellationToken(),httpClient).Result);
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
            GlobalConstans.NullPathUrl.HtmlDownloadAsync(new CancellationToken(), httpClient).Result);
        Assert.ThrowsException<AggregateException>(() =>
            GlobalConstans.NullUrl.HtmlDownloadAsync(new CancellationToken(), httpClient).Result);
    }
    
    
   
}