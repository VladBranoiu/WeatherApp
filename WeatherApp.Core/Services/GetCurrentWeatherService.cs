using WeatherApp.Core.Interfaces;
using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Core.Services;

public sealed class GetCurrentWeatherService(ICurrentWeatherProvider currentWeatherProvider)
{
    public Task<CurrentWeather> ExecuteAsync(
        LocationQuery locationQuery,
        CancellationToken cancellationToken = default)
    {
        if (locationQuery is null)
            throw new ArgumentNullException(nameof(locationQuery));

        var queryText = (locationQuery.QueryText ?? string.Empty).Trim();
        if (queryText.Length < 2)
            throw new ArgumentException("Location query is too short.", nameof(locationQuery));

        var normalizedLocationQuery = new LocationQuery(queryText);

        return currentWeatherProvider.GetCurrentWeatherAsync(normalizedLocationQuery, cancellationToken);
    }
}