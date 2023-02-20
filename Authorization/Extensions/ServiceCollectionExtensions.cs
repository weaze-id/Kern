using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Kern.Authorization.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthorization(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<CookieAuthenticationOptions>? cookieOptions = null,
        Action<JwtBearerOptions>? jwtOptions = null,
        Action<AuthorizationOptions>? authorizationOptions = null)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<AuthenticationService>();

        var multiSchemePolicy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme,
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
            .Configure<SecurityKey>(
                (options, securityKey) =>
                {
                    jwtOptions?.Invoke(options);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = securityKey,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidIssuer = configuration["Jwt:Issuer"],
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

        return services;
    }
}