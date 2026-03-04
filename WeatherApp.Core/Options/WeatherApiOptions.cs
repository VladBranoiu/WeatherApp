namespace WeatherApp.Core.Options;

public sealed class WeatherApiOptions
{
    public string ApiKey { get; init; } = string.Empty;
    public string BaseUrl { get; init; } = "https://api.weatherapi.com/v1/";
    public bool IncludeAirQuality { get; init; } = false;
    public string? LanguageCode { get; init; }
}