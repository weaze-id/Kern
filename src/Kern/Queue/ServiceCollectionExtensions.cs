using Microsoft.Extensions.DependencyInjection;

namespace Kern.Queue;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueue(this IServiceCollection services)
    {
        services.AddSingleton<Queue>();
        services.AddHostedService<QueueHost>();

        return services;
    }
}