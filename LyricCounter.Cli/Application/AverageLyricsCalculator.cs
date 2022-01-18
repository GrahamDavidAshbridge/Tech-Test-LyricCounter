using System.Collections.Concurrent;
using Ardalis.GuardClauses;
using Serilog;

namespace LyricCounter.Cli.Application
{
    internal class AverageLyricsCalculator : IAverageLyricsCalculator
    {
        private readonly ILyricsRetriever _lyricsRetriever;

        public AverageLyricsCalculator(ILyricsRetriever lyricsRetriever)
        {
            _lyricsRetriever = lyricsRetriever;
        }

        public async Task<double> CalculateAverageAsync(string artistName, IReadOnlyList<string> songs)
        {
            var apiSemaphoreThrottler = new SemaphoreSlim(12, 12);
            var lyricSums = new ConcurrentBag<int>();
            var tasks = songs.Select(song => Task.Run(async () =>
                {
                    await apiSemaphoreThrottler.WaitAsync();
                    try
                    {
                        var currentLyricSum = await _lyricsRetriever.GetLyricsCountForSong(artistName, song);
                        Log.Information($"Song {song} has a count of {currentLyricSum} lyrics.");
                        Guard.Against.Zero(currentLyricSum, nameof(currentLyricSum));
                        lyricSums.Add(currentLyricSum);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, $"An error occured calculating lyric for song {song}");
                    }
                    finally
                    {
                        apiSemaphoreThrottler.Release();
                    }
                }))
                .ToList();
            await Task.WhenAll(tasks);
            return Math.Round(lyricSums.Average(), 2);
        }

    }
}