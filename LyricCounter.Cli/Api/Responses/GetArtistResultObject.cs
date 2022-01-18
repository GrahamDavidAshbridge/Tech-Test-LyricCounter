using System.Text.Json.Serialization;

namespace LyricCounter.Cli.Api.Responses
{
    public record GetArtistResultObject
    {
        [JsonPropertyName("primary_artist")]
        public GetArtistPrimaryResponse PrimaryArtist { get; init; }
    }
}