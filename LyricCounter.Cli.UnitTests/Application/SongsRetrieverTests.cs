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

public class SongsRetrieverTests
{
    private Mock<IApi> _api;
    private IConfiguration _configuration;

    //setup
    public SongsRetrieverTests()
    {
        _api = new Mock<IApi>();

        var expectedApiResponsepage1 = new GetSongsByArtistResponse
        {
            Response = new GetSongsByArtistObject
            {
                NextPage = 2,
                Songs = new [] { new GetSongsObject { SongTitle = "test"}, new GetSongsObject { SongTitle = "test2"}}
            }
        };
        var expectedApiResponsepage2 = new GetSongsByArtistResponse
        {
            Response = new GetSongsByArtistObject
            {
                NextPage = null,
                Songs = new [] { new GetSongsObject { SongTitle = "test3"}, new GetSongsObject { SongTitle = "test4"}}
            }
        };
            
        _api.Setup(c => c.GetAsync<GetSongsByArtistResponse>(It.Is<string>(a => a == "Genius"),
            It.Is<string>(a => a == "artists/99/songs?per_page=50&page=1"))).ReturnsAsync(expectedApiResponsepage1);
        

        _api.Setup(c => c.GetAsync<GetSongsByArtistResponse>(It.Is<string>(a => a == "Genius"),
            It.Is<string>(a => a == "artists/99/songs?per_page=50&page=2"))).ReturnsAsync(expectedApiResponsepage2);

        var inMemorySettings = new Dictionary<string, string> {
            {"LyricApiSettings:SongsEndPoint", "artists/{0}/songs?per_page=50&page={1}"},
        };
        
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Fact]
    public async Task RetrieverLyricsAsync_Should_ReturnList()
    {
        var setupObject = new SongsRetriever(_api.Object, _configuration);

        var result = await setupObject.GetSongsByArtistId(99);
        
        result.Count.ShouldBe(4);
    }
    
    [Fact]
    public async Task RetrieverLyricsAsync_Should_ThrowArgumentException()
    {
        var setupObject = new SongsRetriever(_api.Object, _configuration);
        
        await setupObject.GetSongsByArtistId(0).ShouldThrowAsync<ArgumentException>();
    }
    
}