namespace LyricCounter.Cli.Application
{
    internal interface ILyricsRetriever
    {
        Task<int> GetLyricsCountForSong(string artistName, string songName);
    }
}