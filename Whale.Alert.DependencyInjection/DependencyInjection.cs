using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Whale.Alert.Abstractions;

namespace Whale.Alert.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWhaleAlertServices(this IServiceCollection services,
        Action<WhaleAlertOptions> configureOptions)
    {
        services.Configure(configureOptions);
        
        services.AddScoped<IWhaleApiFactory>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<WhaleAlertOptions>>().Value;
            
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(options.BaseAddress)
            };
            
            return new WhaleAlertFactory(httpClient);
        });
        
        services.AddScoped<IWhaleAlert>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<WhaleAlertOptions>>().Value;

            var factory = provider.GetRequiredService<IWhaleApiFactory>();

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(options.BaseAddress)
            };

            var apiKey = options.ApiKey;
            
            return factory.Create(apiKey);
        });

        return services;
    }
}