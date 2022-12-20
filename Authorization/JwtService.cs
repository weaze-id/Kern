using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Kern.Authorization;

public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly SecurityKey _securityKey;

    public JwtService(IConfiguration configuration, SecurityKey securityKey)
    {
        _configuration = configuration;
        _securityKey = securityKey;
    }

    /// <summary>Encode JwtIdentity into jwt string.</summary>
    /// <param name="jwtIdentity">Data to encode.</param>
    /// <returns>Encoded JwtIdentity.</returns>
    public string Encode(IJwtIdentity jwtIdentity, string algorithms)
    {
        var signingCredentials = new SigningCredentials(_securityKey, algorithms);
        var securityToken = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            jwtIdentity.ToClaims(),
            expires: DateTime.UtcNow.AddSeconds(int.Parse(_configuration["Jwt:ExpiredIn"]!)),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}