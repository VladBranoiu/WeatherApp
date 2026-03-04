using System.Text.Json.Serialization;

namespace WeatherApp.Core.Dtos.Request;

public class WeatherApiConditionRequest
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }
}