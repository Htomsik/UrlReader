using Core.Extensions;
using CoreTests.Resources;

namespace CoreTests.ExtensionsTests.UrlExtensions;

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
        Assert.ThrowsException<ArgumentNullException>(() => GlobalConstans.NullUrl.NullChecker());
        Assert.ThrowsException<ArgumentNullException>(() => GlobalConstans.NullPathUrl.NullChecker());
    }

    #endregion
}