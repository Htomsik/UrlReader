using UrlReader.Core.Services.ParserService.Base;
using UrlReader.Tests.Core.Resources;

namespace UrlReader.Tests.Core.Tests.ServicesTests;

[TestClass]
public class TagParserTests
{
    /// <summary>
    ///     Is tags value is right
    /// </summary>
    [TestMethod]
    public void IsParsingRight()
    {
        //Act
        var tagParser = new TagParser();

        ICollection<string> parsingTags;

        //Arrange
        parsingTags = tagParser.Parse(GlobalConstants.HtmlDocumentSample(),"p");

        // Assert
        Assert.AreEqual(4,parsingTags.Count);
    }
}