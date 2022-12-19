using Kern.Authorization.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Kern.Authorization.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPermissionHandler(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        return services;
    }
}