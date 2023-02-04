using Moq;
using UrlReader.Core.Services.FileService.Base;

namespace UrlReader.Tests.Core.Resources;

public static class ServicesMocks
{
    public static IFileService<TValue> CreateFileService<TValue>(TValue output) 
    {
        var mockFileService = new Mock<IFileService<TValue>>();
        
        mockFileService.Setup(x => x.GetDataFromFile()).ReturnsAsync(output);

        return mockFileService.Object;
    }
}