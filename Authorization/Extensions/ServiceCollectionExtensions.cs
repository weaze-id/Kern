using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Kern.Authorization.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthorization(
        this IServiceCollection services,
        Action<CookieAuthenticationOptions>? cookieOptions = null,
        Action<JwtBearerOptions, SecurityKey>? jwtOptions = null,
        Action<AuthorizationOptions>? authorizationOptions = null)
    {
        services.AddHttpContextAccessor();

        var multiSchemePolicy = new AuthorizationPolicyBuilder(
                CookieAuthenticationDefaults.AuthenticationScheme,
                JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();

        services
            .AddAuthentication()
            .AddJwtBearer()
            .AddCookie(options => cookieOptions?.Invoke(options));

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = multiSchemePolicy;
            authorizationOptions?.Invoke(options);
        });

        services
            .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<SecurityKey>((options, securityKey) => jwtOptions?.Invoke(options, securityKey));

        return services;
    }
}