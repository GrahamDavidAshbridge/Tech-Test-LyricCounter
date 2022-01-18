namespace LyricCounter.Cli.Application;

internal interface IAverageLyricsCalculator
{
    Task<double> CalculateAverageAsync(string artistName, IReadOnlyList<string> songs);
}