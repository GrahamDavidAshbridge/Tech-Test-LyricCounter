using System.Text.Json.Serialization;

namespace LyricCounter.Cli.Api.Responses
{
    public record GetArtistSearchResponse
    {
        [JsonPropertyName("response")]
        public GetArtistResponseObject Response { get; init; }
    }
}