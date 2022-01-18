using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LyricCounter.Cli.Api;
using LyricCounter.Cli.Api.Responses;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using LyricCounter.Cli.Application;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using Shouldly;
using Assert = Xunit.Assert;

namespace LyricCounter.Cli.UnitTests.Application;

public class AverageLyricsCalculatorTests
{
    private Mock<ILyricsRetriever> _lyricsRetriever;

    //setup
    public AverageLyricsCalculatorTests()
    {
        _lyricsRetriever = new Mock<ILyricsRetriever>();

        _lyricsRetriever.Setup(a =>
                a.GetLyricsCountForSong(It.Is<string>(b => b == "artist"),
                    It.Is<string>(a => a == "positiveTest")))
            .ReturnsAsync(10);
        
        _lyricsRetriever.Setup(a =>
                a.GetLyricsCountForSong(It.Is<string>(b => b == "artist"),
                    It.Is<string>(a => a == "noResult")))
            .ReturnsAsync(0);
        
        Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
    }

    [Fact]
    public async Task RetrieverArtistIdAsync_Should_ReturnAverage()
    {
        var setupObject = new AverageLyricsCalculator(_lyricsRetriever.Object);

        using (TestCorrelator.CreateContext())
        {
            var result = await setupObject.CalculateAverageAsync("artist", new[] {"positiveTest", "noResult"});
            
            TestCorrelator.GetLogEventStreamFromCurrentContext()
                .Subscribe(logEvent => logEvent.MessageTemplate.Text.ShouldBe("Song positiveTest has a count of 10 lyrics."));
            TestCorrelator.GetLogEventStreamFromCurrentContext()
                .Subscribe(logEvent => logEvent.MessageTemplate.Text.ShouldBe("An error occured calculating lyric for song noResult"));
            
            result.ShouldBe(10);
        }
    }
}