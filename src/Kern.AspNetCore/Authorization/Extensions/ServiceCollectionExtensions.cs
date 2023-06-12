using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Kern.AspNetCore.Authorization.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthorization(
        this IServiceCollection services,
        Action<AuthenticationBuilder> builder,
        IEnumerable<string> authenticationScheme,
        Func<HttpContext, string?> schemeSelector,
        Action<AuthorizationOptions>? authorizationOptions = null)
    {
        const string scheme = "MULTIPLE_AUTHENTICATION_SCHEME";

        var authenticationBuilder = services
            .AddAuthentication(scheme)
            .AddPolicyScheme(scheme, scheme, options => options.ForwardDefaultSelector = schemeSelector);

        builder.Invoke(authenticationBuilder);

        var multiAuthorizationSchemePolicy = new AuthorizationPolicyBuilder(authenticationScheme.ToArray())
            .RequireAuthenticatedUser()
            .Build();

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = multiAuthorizationSchemePolicy;
            authorizationOptions?.Invoke(options);
        });

        return services;
    }
}