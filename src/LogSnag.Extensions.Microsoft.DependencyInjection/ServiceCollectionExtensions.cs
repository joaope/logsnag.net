using Microsoft.Extensions.DependencyInjection;

namespace LogSnag;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddLogSnagHttpClient(this IServiceCollection services, string apiToken)
    {
        if (string.IsNullOrWhiteSpace(apiToken))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiToken));
        }

        return services.AddHttpClient<ILogSnagHttpClient, LogSnagHttpClient>(client => new LogSnagHttpClient(client, apiToken));
    }
}