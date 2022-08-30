using Kern.Internal.Authorization.Metadatas;

namespace Kern.Internal.Authorization.Extensions;

public static class EndpointExtension
{
    public static RouteHandlerBuilder NeedAuthorization(this RouteHandlerBuilder builder, string? permission = null)
    {
        builder.Add(endpointBuilder => endpointBuilder.Metadata.Add(new RequireAuthorizationMetadata(permission)));
        return builder;
    }

    public static RouteHandlerBuilder ValidateAudience(this RouteHandlerBuilder builder, string? audience = null)
    {
        builder.Add(endpointBuilder => endpointBuilder.Metadata.Add(new AudienceMetadata(audience)));
        return builder;
    }
}