using System.Security.Claims;

namespace Kern.Internal.Authorization.Extensions;

public static class ClaimsExtension
{
    /// <summary>Get claim value from claim list.</summary>
    /// <returns>Claim value.</returns>
    public static string? GetValue(this IEnumerable<Claim> claims, string type)
    {
        var claim = claims.FirstOrDefault(e => e.Type == type);
        return claim?.Value;
    }

    /// <summary>Get claim value from claim list.</summary>
    /// <returns>Claim value.</returns>
    public static string? GetValue(this ClaimsPrincipal claimsPrincipal, string type)
    {
        return claimsPrincipal.Claims.GetValue(type);
    }

    /// <summary>Check current user has required permission or not.</summary>
    /// <returns>True if user has required permission.</returns>
    public static bool HasPermission(this IEnumerable<Claim> claims, string permission)
    {
        var permissions = claims.GetValue("Permissions");
        if (string.IsNullOrWhiteSpace(permissions))
        {
            return false;
        }

        var permissionList = permissions.Split(',');
        return permissionList.Contains(permission);
    }

    /// <summary>Check current user has required permission or not.</summary>
    /// <returns>True if user has required permission.</returns>
    public static bool HasPermission(this ClaimsPrincipal claimsPrincipal, string permission)
    {
        return claimsPrincipal.Claims.HasPermission(permission);
    }
}