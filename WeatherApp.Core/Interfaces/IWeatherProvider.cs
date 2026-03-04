using WeatherApp.Domain.Entities;
using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Core.Interfaces;

public interface ICurrentWeatherProvider
{
    Task<CurrentWeather> GetCurrentWeatherAsync(
        LocationQuery locationQuery,
        CancellationToken cancellationToken = default);
}