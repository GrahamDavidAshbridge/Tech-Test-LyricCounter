namespace LyricCounter.Cli.Application
{
    internal interface IArtistRetriever
    {
        Task<int> RetrieverArtistIdAsync(string artistName);
    }
}