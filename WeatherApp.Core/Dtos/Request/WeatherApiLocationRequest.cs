using System.Text.Json.Serialization;

namespace WeatherApp.Core.Dtos.Request;

public class WeatherApiLocationRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }
}
