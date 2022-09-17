using Core.Services.ParserService;
using CoreTests.Resources;

namespace CoreTests.Tests.ServicesTests;

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