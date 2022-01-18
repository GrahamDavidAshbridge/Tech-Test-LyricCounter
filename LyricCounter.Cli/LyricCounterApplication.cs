using LyricCounter.Cli.Application;
using Serilog;

namespace LyricCounter.Cli
{
    internal class LyricCounterApplication
    {
        private readonly ISongsRetriever _songsRetriever;
        private readonly IArtistRetriever _artistRetriever;
        private readonly IAverageLyricsCalculator _averageLyricsCalculator;
        private readonly IConsoleOutput _consoleOutput;
        
        public LyricCounterApplication(ISongsRetriever songsRetriever, 
            IArtistRetriever artistRetriever, 
            IAverageLyricsCalculator averageLyricsCalculator, 
            IConsoleOutput consoleOutput)
        {
            _songsRetriever = songsRetriever;
            _artistRetriever = artistRetriever;
            _consoleOutput = consoleOutput;
            _averageLyricsCalculator = averageLyricsCalculator;
        }
        
        public async Task RunApplicationAsync(string artistName)
        {
            try
            {
                _consoleOutput.WriteLine($"Request for artist {artistName} is processing, please wait.");
                Log.Information($"running for artist {artistName}");
                var artistId = await _artistRetriever.RetrieverArtistIdAsync(artistName);
                Log.Information($"artist id found {artistId}");
                var artistSongs = await _songsRetriever.GetSongsByArtistId(artistId);
                Log.Information($"artist songs pulled with a count of {artistSongs.Count()}");
                var average = await _averageLyricsCalculator.CalculateAverageAsync(artistName, artistSongs);
                Log.Information($"artists average found at {average}");
                _consoleOutput.WriteLine($"{artistName} has an average lyric count of {average}");
            }
            catch (Exception e)
            {
                Log.Error(e, $"Failure for artist {artistName}");
                _consoleOutput.WriteLine($"An error occured calculating your average for artist {artistName} - {e.Message}");
            }
        }
    }
}