using LyricCounter.Cli.Api;
using Microsoft.Extensions.Configuration;
using Ardalis.GuardClauses;
using LyricCounter.Cli.Api.Responses;

namespace LyricCounter.Cli.Application
{
    internal class ArtistRetriever : IArtistRetriever
    {
        private readonly IApi _api;
        private readonly IConfiguration _configuration;

        public ArtistRetriever(IApi api, IConfiguration configuration)
        {
            _configuration = configuration;
            _api = api;
        }

        public async Task<int> RetrieverArtistIdAsync(string artistName)
        {
            Guard.Against.NullOrWhiteSpace(artistName, nameof(artistName));
            var uri = string.Format(_configuration["LyricApiSettings:SearchEndPoint"], artistName);
            var artistSearchResult = await _api.GetAsync<GetArtistSearchResponse>("Genius", uri);
            Guard.Against.Null(artistSearchResult, nameof(artistSearchResult));
            var artist = artistSearchResult.Response.Hits.FirstOrDefault(x =>
                string.Equals(x.ResultObject.PrimaryArtist.ArtistName, artistName, StringComparison.OrdinalIgnoreCase));
            Guard.Against.Null(artist, nameof(artist));
            return artist.ResultObject.PrimaryArtist.ArtistId;
        }
    }
}