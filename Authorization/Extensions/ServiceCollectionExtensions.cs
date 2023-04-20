using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

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

        const string multiAuthenticationSchemeName = "MULTIPLE_AUTHENTICATION_SCHEME";

        services
            .AddAuthentication(multiAuthenticationSchemeName)
            .AddJwtBearer()
            .AddCookie(options => cookieOptions?.Invoke(options))
            .AddPolicyScheme(multiAuthenticationSchemeName, multiAuthenticationSchemeName, options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    string? authorizationHeader = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                    {
                        return JwtBearerDefaults.AuthenticationScheme;
                    }

                    return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            });


        var multiAuthorizationSchemePolicy = new AuthorizationPolicyBuilder(
                CookieAuthenticationDefaults.AuthenticationScheme,
                JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = multiAuthorizationSchemePolicy;
            authorizationOptions?.Invoke(options);
        });

        services
            .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<SecurityKey>((options, securityKey) => jwtOptions?.Invoke(options, securityKey));

        return services;
    }
}