using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Core.Services;

namespace WeatherApp.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<GetCurrentWeatherService>();
        return services;
    }
}