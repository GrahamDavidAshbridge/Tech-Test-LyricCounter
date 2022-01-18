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

public class ArtistRetrieverTests
{
    private Mock<IApi> _api;
    private IConfiguration _configuration;

    //setup
    public ArtistRetrieverTests()
    {
        _api = new Mock<IApi>();
        
        var expectedApiResponse = new GetArtistSearchResponse
        {
            Response = new GetArtistResponseObject
            {
                Hits = new [] { new GetArtistHitsResponse 
                { ResultObject = 
                    new GetArtistResultObject{PrimaryArtist = 
                        new GetArtistPrimaryResponse{ ArtistId = 99, ArtistName = "unitTestValue"}} } }
            }
        };
        _api.Setup(c => c.GetAsync<GetArtistSearchResponse>(It.Is<string>(a => a == "Genius"),
            It.Is<string>(a => a == "search?q=unitTestValue"))).ReturnsAsync(expectedApiResponse);
        
        var unexpectedSearchResult = new GetArtistSearchResponse
        {
            Response = new GetArtistResponseObject
            {
                Hits = new [] { new GetArtistHitsResponse 
                { ResultObject = 
                    new GetArtistResultObject{PrimaryArtist = 
                        new GetArtistPrimaryResponse{ ArtistId = 99, ArtistName = "wrongMatch"}} } }
            }
        };
        _api.Setup(c => c.GetAsync<GetArtistSearchResponse>(It.Is<string>(a => a == "Genius"),
            It.Is<string>(a => a == "search?q=correctmatch"))).ReturnsAsync(unexpectedSearchResult);

        
        var inMemorySettings = new Dictionary<string, string> {
            {"LyricApiSettings:SearchEndPoint", "search?q={0}"},
        };
        
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Fact]
    public async Task RetrieverArtistIdAsync_Should_ReturnArtistId()
    {
        var setupObject = new ArtistRetriever(_api.Object, _configuration);

        var result = await setupObject.RetrieverArtistIdAsync("unitTestValue");
        
        result.ShouldBe(99);
    }
    
    [Fact]
    public async Task RetrieverArtistIdAsync_Should_ThrowArgumentException()
    {
        var setupObject = new ArtistRetriever(_api.Object, _configuration);

        await setupObject.RetrieverArtistIdAsync(string.Empty).ShouldThrowAsync<ArgumentException>();
    }
    
    [Fact]
    public async Task RetrieverArtistIdAsync_Should_ThrowArgumentExceptionResult()
    {
        var setupObject = new ArtistRetriever(_api.Object, _configuration);
        
        await setupObject.RetrieverArtistIdAsync(string.Empty).ShouldThrowAsync<ArgumentException>("unknown artist");
    }
    
    [Fact]
    public async Task RetrieverArtistIdAsync_Should_ThrowArgumentExceptionOnIncorrectSearchResult()
    {
        var setupObject = new ArtistRetriever(_api.Object, _configuration);

        await setupObject.RetrieverArtistIdAsync("correctmatch").ShouldThrowAsync<ArgumentException>();
    }
}