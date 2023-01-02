using Microsoft.Extensions.DependencyInjection;

namespace LogSnag;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddLogSnagHttpClient(this IServiceCollection services) => 
        services.AddHttpClient<ILogSnagHttpClient, LogSnagHttpClient>();
}