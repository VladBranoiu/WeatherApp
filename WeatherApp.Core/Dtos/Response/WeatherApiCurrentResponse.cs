using System.Text.Json.Serialization;
using WeatherApp.Core.Dtos.Request;

namespace WeatherApp.Core.Dtos.Response;

public class WeatherApiCurrentResponse
{
    [JsonPropertyName("location")]
    public WeatherApiLocationRequest? Location { get; set; }

    [JsonPropertyName("current")]
    public WeatherApiCurrentRequest? Current { get; set; }
}