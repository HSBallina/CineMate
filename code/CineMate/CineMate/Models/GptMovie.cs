using System.Text.Json.Serialization;

namespace CineMate.Models;

public class GptMovie
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }
    [JsonPropertyName("year")]
    public int Year { get; set; }
}
