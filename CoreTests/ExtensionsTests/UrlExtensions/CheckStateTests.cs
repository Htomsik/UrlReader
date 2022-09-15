using System.Net;
using Core.Extensions;
using Core.Models;
using CoreTests.Resources;

namespace CoreTests.ExtensionsTests.UrlExtensions;

[TestClass]
public class CheckStateTests
{
    #region CheckState

    /// <summary>
    ///     Is CheckState returns UrlState.Alive when httpResponse is ok
    /// </summary>
    [TestMethod]
    public void IsAlive()
    {
        //Arrange
        var httpClient = HttpMocks.CreateHttpClient(HttpStatusCode.OK);

        //Act+Assert
        Assert.AreEqual(UrlState.Alive,
            GlobalConstans.RightUrl.CheckState(new CancellationToken(), httpClient).Result);
    }


    /// <summary>
    ///     Is CheckState returns UrlState.NotAlive when httpResponse is Conflict
    /// </summary>
    [TestMethod]
    public void IsNotAlive()
    {
        //Arrange
        var httpClient = HttpMocks.CreateHttpClient(HttpStatusCode.Conflict);

        //Act+Assert
        Assert.AreEqual(UrlState.NotAlive,
            GlobalConstans.RightUrl.CheckState(new CancellationToken(), httpClient).Result);
    }

    /// <summary>
    ///     Is CheckState returns UrlState.Unknown when path have errors
    /// </summary>
    [TestMethod]
    public void IsUnknownPath()
    {
        //Arrange
        var httpClient = HttpMocks.CreateHttpClient(HttpStatusCode.OK);

        //Act+Assert
        Assert.AreEqual(UrlState.Unknown,
            GlobalConstans.ErrorUrl.CheckState(new CancellationToken(), httpClient).Result);
    }

    /// <summary>
    ///     Is CheckState returns UrlState.Unknown when end TimeOut
    /// </summary>
    [TestMethod]
    public void IsUnknownTimeout()
    {
        //Arrange
        var httpClient = HttpMocks.CreateHttpClient(HttpStatusCode.OK,"",true);

        //Act+Assert
        Assert.AreEqual(UrlState.Unknown,
            GlobalConstans.RightUrl.CheckState(new CancellationToken(), httpClient).Result);
    }


    /// <summary>
    ///     Is CheckState throw exception when Url or Url.path is null
    /// </summary>
    [TestMethod]
    public void IsThrowExceptionIfUrlOrPathEmpty()
    {
        //Arrange
        var httpClient = HttpMocks.CreateHttpClient(HttpStatusCode.OK);

        //Act+Assert
        Assert.ThrowsException<AggregateException>(() =>
            GlobalConstans.NullPathUrl.CheckState(new CancellationToken(), httpClient).Result);
        Assert.ThrowsException<AggregateException>(() =>
            GlobalConstans.NullUrl.CheckState(new CancellationToken(), httpClient).Result);
    }

    #endregion
}