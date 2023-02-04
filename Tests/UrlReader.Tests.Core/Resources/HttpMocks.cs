using System.Net;
using Moq;
using Moq.Protected;

namespace UrlReader.Tests.Core.Resources;

/// <summary>
///     generate Http modules mocks
/// </summary>
internal static class HttpMocks
{
    public static Mock<HttpMessageHandler> CreateMockHttpMessageHandler(HttpResponseMessage response,
        bool isTimeOutEnd = false)
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() =>
            {
                if (isTimeOutEnd) throw new OperationCanceledException();

                return response;
            });

        return handlerMock;
    }

    public static HttpClient CreateHttpClient(HttpStatusCode statusCode, string returnedContent = " ",
        bool isTimeOutEnd = false) => new 
        (
            CreateMockHttpMessageHandler
            (
                new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(returnedContent)
                },
                isTimeOutEnd
            ).Object    
        );

}