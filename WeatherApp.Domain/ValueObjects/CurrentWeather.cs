namespace WeatherApp.Domain.ValueObjects;

public sealed record CurrentWeather(
    string LocationName,
    string CountryName,
    double TemperatureCelsius,
    string ConditionText,
    string ConditionIconUrl,
    int HumidityPercentage,
    double WindKph
);