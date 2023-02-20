using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Kern.Authorization;

public class AuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly SecurityKey _securityKey;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(IConfiguration configuration, SecurityKey securityKey, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _securityKey = securityKey;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> SignInAsync(IIdentity jwtIdentity, string jwtAlgorithm)
    {
        var claims = jwtIdentity.ToClaims();
        var httpContext = _httpContextAccessor.HttpContext!;

        httpContext.User.AddIdentity(new ClaimsIdentity(claims));
        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, httpContext.User);

        var signingCredentials = new SigningCredentials(_securityKey, jwtAlgorithm);
        var securityToken = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddSeconds(int.Parse(_configuration["Jwt:ExpiredIn"]!)),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public Task SignOutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        return httpContext.SignOutAsync();
    }
}