namespace LyricCounter.Cli.Application;

internal interface ISongsRetriever
{
    Task<IReadOnlyList<string>> GetSongsByArtistId(int artistId);
}