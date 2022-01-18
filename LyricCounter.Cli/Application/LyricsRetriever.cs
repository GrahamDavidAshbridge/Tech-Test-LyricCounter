using Ardalis.GuardClauses;
using LyricCounter.Cli.Api;
using LyricCounter.Cli.Api.Responses;
using Microsoft.Extensions.Configuration;

namespace LyricCounter.Cli.Application
{
    public class LyricsRetriever : ILyricsRetriever
    {
        private readonly IApi _api;
        private readonly IConfiguration _configuration;

        public LyricsRetriever(IApi api, IConfiguration configuration)
        {
            _configuration = configuration;
            _api = api;
        }

        public async Task<int> GetLyricsCountForSong(string artistName, string songName)
        {
            var uri = string.Format(_configuration["LyricsOvhApiSettings:GetLyricEndpoint"], artistName, songName);
            var song = await _api.GetAsync<GetLyricsResponse>("LyricsOvh", uri);
            Guard.Against.NullOrEmpty(song.Lyrics, nameof(song.Lyrics));
            return song.Lyrics.Split(new char[] {' ', '\r', '\n' },StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}