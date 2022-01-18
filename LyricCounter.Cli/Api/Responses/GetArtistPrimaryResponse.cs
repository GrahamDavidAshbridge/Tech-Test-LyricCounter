using System.Text.Json.Serialization;

namespace LyricCounter.Cli.Api.Responses
{
    public record GetArtistPrimaryResponse
    {
        [JsonPropertyName("name")]
        public string ArtistName { get; init; }
        
        [JsonPropertyName("id")]
        public int ArtistId { get; init; }
    }
}