using Core.Extensions;
using CoreTests.Resources;

namespace CoreTests.Tests.ExtensionsTests.UrlExtensions;

[TestClass]
public class NullCheckerTests
{
    #region NullChecker

    /// <summary>
    ///     Is NullChecker throw exception when Url or Url.path is null
    /// </summary>
    [TestMethod]
    public void UrlNullCheckerThrowExceptions()
    {
        //Act+Arrange+Assert
        Assert.ThrowsException<ArgumentNullException>(() => GlobalConstants.NullUrl.NullChecker());
        Assert.ThrowsException<ArgumentNullException>(() => GlobalConstants.NullPathUrl.NullChecker());
    }

    #endregion
}