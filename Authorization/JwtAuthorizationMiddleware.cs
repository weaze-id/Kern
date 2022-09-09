using System.Security.Claims;
using Kern.Internal.Authorization.Extensions;
using Kern.Internal.Authorization.Metadatas;
using Kern.Internal.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Kern.Internal.Authorization;

public static class JwtAuthorizationMiddleware
{
    public static IApplicationBuilder UseAuthorization(this IApplicationBuilder app)
    {
        app.Use(MiddlewareAsync);
        return app;
    }

    private static async Task MiddlewareAsync(HttpContext context, RequestDelegate next)
    {
        var metadata = context.GetEndpoint()?.Metadata;

        // Check if endpoint need authorization.
        var requireAuthorization =
            metadata?.FirstOrDefault(e => e.GetType() == typeof(RequireAuthorizationMetadata)) as
                RequireAuthorizationMetadata;
        if (requireAuthorization == null)
        {
            await next.Invoke(context);
            return;
        }

        // Get claims from validated bearer token.
        var claims = ParseJwtToClaims(context);
        if (claims == null)
        {
            await JsonResponse.Unauthorized().ExecuteAsync(context);
            return;
        }

        // Check audience.
        var audience = metadata?.FirstOrDefault(e => e.GetType() == typeof(AudienceMetadata)) as AudienceMetadata;
        if (audience != null && audience.Audience != claims.GetValue("aud"))
        {
            var errorMessage = "Can't access to the resource using current JWT.";
            await JsonResponse.Forbidden(errors: errorMessage).ExecuteAsync(context);
            return;
        }

        // Check user claim contains require permission.
        if (requireAuthorization.Permission != null && !claims.HasPermission(requireAuthorization.Permission))
        {
            var errorMessage = $"{requireAuthorization.Permission} permission is required to access the resource.";
            await JsonResponse.Forbidden(errors: errorMessage).ExecuteAsync(context);
            return;
        }

        context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "JwtAuthentication"));
        await next.Invoke(context);
    }

    private static IEnumerable<Claim>? ParseJwtToClaims(HttpContext context)
    {
        var authorizationHeader = context.Request
            .Headers["Authorization"]
            .FirstOrDefault();

        if (authorizationHeader == null)
        {
            return null;
        }

        // Split authorizationHeader, and validate first string is "Bearer".
        var authorizationHeaderSplit = authorizationHeader.Split();
        if (authorizationHeaderSplit.Length != 2 || authorizationHeaderSplit[0] != "Bearer")
        {
            return null;
        }

        var jwtService = context.RequestServices.GetService(typeof(JwtService)) as JwtService;
        return jwtService!.Decode(authorizationHeaderSplit[1]);
    }
}