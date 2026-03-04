using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;
using WeatherApp.Core.Dtos.Response;
using WeatherApp.Core.Helpers;
using WeatherApp.Core.Interfaces;
using WeatherApp.Core.Options;
using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Core.ExternalApis.WeatherApi;

public sealed class WeatherApiCurrentWeatherProvider : ICurrentWeatherProvider
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions _options;

    public WeatherApiCurrentWeatherProvider(HttpClient httpClient, IOptions<WeatherApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;

        if (string.IsNullOrWhiteSpace(_options.ApiKey))
            throw new InvalidOperationException("Missing WeatherApi:ApiKey configuration.");
    }

    public async Task<CurrentWeather> GetCurrentWeatherAsync(
        LocationQuery locationQuery,
        CancellationToken cancellationToken = default)
    {
        if (locationQuery is null)
            throw new ArgumentNullException(nameof(locationQuery));

        var queryText = (locationQuery.QueryText ?? string.Empty).Trim();
        if (queryText.Length < 2)
            throw new ArgumentException("Location query is too short.", nameof(locationQuery));

        queryText = queryText.RemoveDiacritics();

        var airQualityFlag = _options.IncludeAirQuality ? "yes" : "no";

        var requestUrl =
            $"current.json?key={Uri.EscapeDataString(_options.ApiKey)}&q={Uri.EscapeDataString(queryText)}&aqi={airQualityFlag}";

        using var httpResponse = await _httpClient.GetAsync(requestUrl, cancellationToken);

        if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
            throw new InvalidOperationException("WeatherAPI unauthorized (check API key).");

        if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
            throw new InvalidOperationException("WeatherAPI bad request (check query text).");

        httpResponse.EnsureSuccessStatusCode();

        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<WeatherApiCurrentResponse>(
            cancellationToken: cancellationToken);

        if (apiResponse?.Location is null || apiResponse.Current is null)
            throw new InvalidOperationException("WeatherAPI returned an unexpected payload.");

        var locationName = apiResponse.Location.Name ?? queryText;
        var countryName = apiResponse.Location.Country ?? string.Empty;

        var conditionText = apiResponse.Current.Condition?.Text ?? string.Empty;
        var conditionIcon = apiResponse.Current.Condition?.Icon ?? string.Empty;

        var conditionIconUrl = NormalizeIconUrl(conditionIcon);

        return new CurrentWeather(
            LocationName: locationName,
            CountryName: countryName,
            TemperatureCelsius: apiResponse.Current.TempCelsius,
            ConditionText: conditionText,
            ConditionIconUrl: conditionIconUrl,
            HumidityPercentage: apiResponse.Current.HumidityPercentage,
            WindKph: apiResponse.Current.WindKph
        );
    }

    private static string NormalizeIconUrl(string iconValue)
    {
        if (string.IsNullOrWhiteSpace(iconValue))
            return string.Empty;

        if (iconValue.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            iconValue.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return iconValue;

        if (iconValue.StartsWith("//"))
            return "https:" + iconValue;

        return "https://" + iconValue.TrimStart('/');
    }
}