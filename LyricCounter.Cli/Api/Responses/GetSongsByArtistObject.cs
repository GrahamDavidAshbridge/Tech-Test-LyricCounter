using System.Text.Json.Serialization;

namespace LyricCounter.Cli.Api.Responses
{
    public record GetSongsByArtistObject
    {
        [JsonPropertyName("next_page")]
        public int? NextPage { get; set; }
        
        [JsonPropertyName("songs")]
        public IReadOnlyList<GetSongsObject> Songs { get; init; }
    }
}