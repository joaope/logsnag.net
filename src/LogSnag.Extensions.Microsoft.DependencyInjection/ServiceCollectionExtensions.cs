using Microsoft.Extensions.DependencyInjection;

namespace LogSnag;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddLogSnagClient(this IServiceCollection services, string apiToken)
    {
        if (string.IsNullOrWhiteSpace(apiToken))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiToken));
        }

        return services.AddHttpClient<ILogSnagClient, LogSnagClient>(client => new LogSnagClient(client, apiToken));
    }
}