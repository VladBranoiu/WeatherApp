using System.Text.Json.Serialization;

namespace WeatherApp.Core.Dtos.Request;

public class WeatherApiCurrentRequest
{
    [JsonPropertyName("temp_c")]
    public double TempCelsius { get; set; }

    [JsonPropertyName("humidity")]
    public int HumidityPercentage { get; set; }

    [JsonPropertyName("wind_kph")]
    public double WindKph { get; set; }

    [JsonPropertyName("condition")]
    public WeatherApiConditionRequest? Condition { get; set; }
}