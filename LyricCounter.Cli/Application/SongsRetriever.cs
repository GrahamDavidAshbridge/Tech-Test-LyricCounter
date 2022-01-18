using Ardalis.GuardClauses;
using LyricCounter.Cli.Api;
using LyricCounter.Cli.Api.Responses;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace LyricCounter.Cli.Application
{
    internal class SongsRetriever : ISongsRetriever
    {
        private readonly IApi _api;
        private readonly IConfiguration _configuration;

        public SongsRetriever(IApi api, IConfiguration configuration)
        {
            _configuration = configuration;
            _api = api;
        }
        
        public async Task<IReadOnlyList<string>> GetSongsByArtistId(int artistId)
        {
            var listOfSongs = new List<string>();
            Guard.Against.NegativeOrZero(artistId, nameof(artistId));
            int? currentPageNumber = 1;
            do
            {
                var uri = string.Format(_configuration["LyricApiSettings:SongsEndPoint"], artistId, currentPageNumber);
                var pageOfSongs = await _api.GetAsync<GetSongsByArtistResponse>("Genius", uri);
                listOfSongs.AddRange(pageOfSongs.Response.Songs.Select(song => song.SongTitle));
                currentPageNumber = pageOfSongs.Response.NextPage;
            } while (currentPageNumber is not null);
            Log.Information($"{listOfSongs.Count} songs found for {artistId}");
            return listOfSongs;
        }
    }
}