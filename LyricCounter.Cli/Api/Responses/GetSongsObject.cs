using System.Text.Json.Serialization;

namespace LyricCounter.Cli.Api.Responses
{
    public record GetSongsObject
    {
        [JsonPropertyName("title")]
        public string SongTitle { get; set; }
    }
}