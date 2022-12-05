using Kern.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Kern.Authorization.Extensions;

public static class AuthorizationOptionsExtensions
{
    public static void AddPermissionPolicy(this AuthorizationOptions options, string permission)
    {
        options.AddPolicy($"{permission}_PERMISSION_POLICY",
            policy => policy.Requirements.Add(new PermissionRequirement(permission)));
    }

    public static void AddAudiencesPolicy(this AuthorizationOptions options, params string[] audiences)
    {
        options.AddPolicy("AUDIENCE_POLICY", policy => policy.RequireClaim("aud", audiences));
        options.DefaultPolicy = options.GetPolicy("AUDIENCE_POLICY") ?? options.DefaultPolicy;
    }
}