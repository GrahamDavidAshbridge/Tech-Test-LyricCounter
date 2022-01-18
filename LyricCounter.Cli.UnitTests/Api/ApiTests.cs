using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LyricCounter.Cli.Api.Responses;
using Moq;
using Moq.Protected;
using Shouldly;
using Xunit;

namespace LyricCounter.Cli.UnitTests.Api;

public class ApiTests
{
    Mock<IHttpClientFactory> mockFactory;

    //setup
    public ApiTests()
    {
        mockFactory = new Mock<IHttpClientFactory>();
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"lyrics\": \"Here the lyrics of the song\"}"),
            });
        var client = new HttpClient(mockHttpMessageHandler.Object);
        client.BaseAddress = new Uri("https://test.com");
        mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
    }

    [Fact]
    public async Task GetAsync_ReturnsT()
    {
        var testObject = new Cli.Api.Api(mockFactory.Object);
        var getResult = await testObject.GetAsync<GetLyricsResponse>("Genius", "testuri");
        getResult.ShouldBeOfType<GetLyricsResponse>();
    }
}