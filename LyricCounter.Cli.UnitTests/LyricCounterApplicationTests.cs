using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LyricCounter.Cli.Application;
using Moq;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using Shouldly;
using Xunit;

namespace LyricCounter.Cli.UnitTests;

public class LyricCounterApplicationTests
{
    private Mock<ISongsRetriever> _songsRetriever;
    private Mock<IArtistRetriever> _artistRetriever;
    private Mock<IAverageLyricsCalculator> _averageLyricsCalculator;
    private Mock<IConsoleOutput> _consoleOutput;
    
    //setup
    public LyricCounterApplicationTests()
    {
        _songsRetriever = new Mock<ISongsRetriever>();
        _artistRetriever = new Mock<IArtistRetriever>();
        _averageLyricsCalculator = new Mock<IAverageLyricsCalculator>();
        _consoleOutput = new Mock<IConsoleOutput>();

        _songsRetriever.Setup(a => a.GetSongsByArtistId(It.IsAny<int>())).ReturnsAsync(new List<string> {"1", "2"});
        _artistRetriever.Setup(a => a.RetrieverArtistIdAsync(It.Is<string>(c => c == "test"))).ReturnsAsync(99);
        _artistRetriever.Setup(a => a.RetrieverArtistIdAsync(It.Is<string>(c => c == "badtest")))
            .ThrowsAsync(new Exception("lorem ipsum"));
        _averageLyricsCalculator.Setup(a => a.CalculateAverageAsync(It.IsAny<string>(), It.IsAny<List<string>>()))
            .ReturnsAsync(10.00);
        _consoleOutput.Setup(a => a.WriteLine(It.IsAny<string>()));

        Log.Logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();

    }
    
    [Fact]
    public async Task RetrieverLyricsAsync_Should_ReturnVoid()
    {
        using (TestCorrelator.CreateContext())
        {
            var result = new LyricCounterApplication(_songsRetriever.Object, _artistRetriever.Object,
                _averageLyricsCalculator.Object, _consoleOutput.Object);

            await result.RunApplicationAsync("test");
            
            TestCorrelator.GetLogEventStreamFromCurrentContext()
                    .Subscribe(logEvent => logEvent.MessageTemplate.Text.ShouldBe("running for artist test"));
            TestCorrelator.GetLogEventStreamFromCurrentContext()
                .Subscribe(logEvent => logEvent.MessageTemplate.Text.ShouldBe("artist id found 99"));
            TestCorrelator.GetLogEventStreamFromCurrentContext()
                .Subscribe(logEvent => logEvent.MessageTemplate.Text.ShouldBe("artist songs pulled with a count of 2"));
            TestCorrelator.GetLogEventStreamFromCurrentContext()
                .Subscribe(logEvent => logEvent.MessageTemplate.Text.ShouldBe("artists average found at 10.00"));
            
            _consoleOutput.Verify(a => a.WriteLine("test has an average lyric count of 10"), Times.Once);
        }
    }
    
    [Fact]
    public async Task RetrieverLyricsAsync_Should_HandleException_ReturnVoid()
    {
        using (TestCorrelator.CreateContext())
        {
            
            var result = new LyricCounterApplication(_songsRetriever.Object, _artistRetriever.Object,
                _averageLyricsCalculator.Object, _consoleOutput.Object);

            await result.RunApplicationAsync("badtest");
            
            TestCorrelator.GetLogEventStreamFromCurrentContext()
                .Subscribe(logEvent => logEvent.MessageTemplate.Text.ShouldBe("Failure for artist badtest"));

            _consoleOutput.Verify(a => a.WriteLine("An error occured calculating your average for artist badtest - lorem ipsum"), Times.Once);
        }
    }
}