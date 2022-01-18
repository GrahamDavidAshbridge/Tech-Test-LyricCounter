using System.Text.Json.Serialization;

namespace LyricCounter.Cli.Api.Responses
{
    public record GetArtistResponseObject
    {
        [JsonPropertyName("hits")]
        public IReadOnlyList<GetArtistHitsResponse> Hits { get; init; }
    }
}