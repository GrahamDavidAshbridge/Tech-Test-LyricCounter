using System.Text.Json.Serialization;

namespace LyricCounter.Cli.Api.Responses
{
    public record GetLyricsResponse
    {
        [JsonPropertyName("lyrics")]
        public string Lyrics { get; init; }
    }
}