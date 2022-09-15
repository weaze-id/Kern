using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Kern.Authorization;

public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly RsaSecurityKey _rsaSecurityKey;

    public JwtService(RsaSecurityKey rsaSecurityKey, IConfiguration configuration)
    {
        _rsaSecurityKey = rsaSecurityKey;
        _configuration = configuration;
    }

    /// <summary>Encode JwtIdentity into jwt string.</summary>
    /// <param name="jwtIdentity">Data to encode.</param>
    /// <returns>Encoded JwtIdentity.</returns>
    public string Encode(IJwtIdentity jwtIdentity)
    {
        var signingCredentials = new SigningCredentials(_rsaSecurityKey, SecurityAlgorithms.RsaSha256);

        var securityToken = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            claims: jwtIdentity.ToClaims(),
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    /// <summary>Decode jwt string in JwtIdentity.</summary>
    /// <param name="jwt">Data to decode.</param>
    /// <returns>Decoded jwt string.</returns>
    public IEnumerable<Claim>? Decode(string jwt)
    {
        // Validate bearer token.
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            RequireSignedTokens = true,
            RequireExpirationTime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _rsaSecurityKey
        };

        try
        {
            tokenHandler.ValidateToken(jwt, tokenValidationParameters, out var validatedToken);
            var jwtSecurityToken = validatedToken as JwtSecurityToken;
            if (jwtSecurityToken == null)
            {
                return null;
            }

            return jwtSecurityToken.Claims;
        }
        catch
        {
            return null;
        }
    }
}