using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Kern.Authorization;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var rsa = RSA.Create(2048);

        services.AddSingleton(_ => new RsaSecurityKey(rsa));
        services.AddScoped<JwtService>();

        return services;
    }
}