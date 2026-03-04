using WeatherApp.Core.ExternalApis.WeatherApi;
using WeatherApp.Core.Interfaces;
using WeatherApp.Core.Options;
using WeatherApp.Core.Services;

namespace WeatherApp.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<GetCurrentWeatherService>();

        services.Configure<WeatherApiOptions>(configuration.GetSection("WeatherApi"));

        services.AddHttpClient<ICurrentWeatherProvider, WeatherApiCurrentWeatherProvider>((serviceProvider, httpClient) =>
        {
            var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<WeatherApiOptions>>().Value;

            // If BaseUrl is not set, fallback to official base.
            var baseUrl = string.IsNullOrWhiteSpace(options.BaseUrl)
                ? "https://api.weatherapi.com/v1/"
                : options.BaseUrl;

            httpClient.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
        });

        return services;
    }
}
