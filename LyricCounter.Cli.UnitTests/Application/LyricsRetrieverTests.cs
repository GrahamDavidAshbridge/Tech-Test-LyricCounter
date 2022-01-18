using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LyricCounter.Cli.Api;
using LyricCounter.Cli.Api.Responses;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using LyricCounter.Cli.Application;
using Shouldly;
using Assert = Xunit.Assert;

namespace LyricCounter.Cli.UnitTests.Application;

public class LyricsRetreiverTests
{
    private Mock<IApi> _api;
    private IConfiguration _configuration;

    //setup
    public LyricsRetreiverTests()
    {
        _api = new Mock<IApi>();

        var expectedApiResponse = new GetLyricsResponse {Lyrics = "its in your head"};
            
        _api.Setup(c => c.GetAsync<GetLyricsResponse>(It.Is<string>(a => a == "LyricsOvh"),
            It.Is<string>(a => a == "artist/unitTestValue"))).ReturnsAsync(expectedApiResponse);
        
        var unexpectedSearchResult = new GetLyricsResponse {Lyrics = ""};

        _api.Setup(c => c.GetAsync<GetLyricsResponse>(It.Is<string>(a => a == "LyricsOvh"),
            It.Is<string>(a => a == "artist/nomatch"))).ReturnsAsync(unexpectedSearchResult);

        var inMemorySettings = new Dictionary<string, string> {
            {"LyricsOvhApiSettings:GetLyricEndpoint", "{0}/{1}"},
        };
        
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Fact]
    public async Task RetrieverLyricsAsync_Should_ReturnCount()
    {
        var setupObject = new LyricsRetriever(_api.Object, _configuration);

        var result = await setupObject.GetLyricsCountForSong("artist", "unitTestValue");
        
        result.ShouldBe(4);
    }
    
    [Fact]
    public async Task RetrieverLyricsAsync_Should_ThrowArgumentException()
    {
        var setupObject = new LyricsRetriever(_api.Object, _configuration);

        await setupObject.GetLyricsCountForSong("artist", "nomatch").ShouldThrowAsync<ArgumentException>();

    }
}