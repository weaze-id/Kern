using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Kern.Internal.Authorization;

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