using System.Text.Json.Serialization;

namespace LyricCounter.Cli.Api.Responses
{
    public record GetSongsByArtistResponse
    {
        [JsonPropertyName("response")]
        public GetSongsByArtistObject Response { get; init; }
    }
}