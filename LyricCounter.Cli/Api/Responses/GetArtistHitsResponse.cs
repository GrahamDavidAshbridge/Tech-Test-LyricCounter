using System.Text.Json.Serialization;

namespace LyricCounter.Cli.Api.Responses
{
    public record GetArtistHitsResponse
    {
        [JsonPropertyName("result")]
        public GetArtistResultObject ResultObject { get; init; }
    }
}