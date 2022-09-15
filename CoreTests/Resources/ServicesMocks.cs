using Core.Services.FileService;
using Moq;

namespace CoreTests.Resources;

public static class ServicesMocks
{
    public static IFileService<TValue> CreateFileService<TValue>(TValue output) 
    {
        var mockFileService = new Mock<IFileService<TValue>>();
        
        mockFileService.Setup(x => x.GetDataFromFile()).Returns(output);

        return mockFileService.Object;
    }
}