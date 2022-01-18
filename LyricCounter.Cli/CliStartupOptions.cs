using CommandLine;

namespace LyricCounter.Cli;

public class CliStartupOptions
{
    [Option('a', "artist", Required = true,
        HelpText = "Enter the artist name you would like to perform average calculation on")]
    public string ArtistName { get; init; }
}