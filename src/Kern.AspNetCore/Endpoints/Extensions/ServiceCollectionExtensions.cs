using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Kern.AspNetCore.Endpoints.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly List<IEndpoints> endpoints = new();

    public static IServiceCollection AddEndpoints<T>(this IServiceCollection services) where T : IEndpoints
    {
        var endpoint = Activator.CreateInstance<T>();
        endpoints.Add(endpoint);

        return services;
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder route)
    {
        for (var i = 0; i < endpoints.Count; i++)
        {
            var endpoint = endpoints[i];
            endpoint.MapEndpoints(route);
        }

        return route;
    }
}