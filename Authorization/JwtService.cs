using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Kern.Authorization;

public class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>Encode JwtIdentity into jwt string.</summary>
    /// <param name="jwtIdentity">Data to encode.</param>
    /// <returns>Encoded JwtIdentity.</returns>
    public string Encode(IJwtIdentity jwtIdentity)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
            SecurityAlgorithms.HmacSha512);

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