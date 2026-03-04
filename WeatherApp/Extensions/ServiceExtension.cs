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

            var baseUrlRaw = options.BaseUrl ?? string.Empty;
            var baseUrl = baseUrlRaw.Trim().Trim('"'); 

            if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var baseUri))
            {
                baseUri = new Uri("https://api.weatherapi.com/v1/", UriKind.Absolute);
            }

            httpClient.BaseAddress = baseUri;

            httpClient.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
        });

        return services;
    }
}