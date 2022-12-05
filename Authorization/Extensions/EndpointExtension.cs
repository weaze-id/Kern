using Microsoft.AspNetCore.Builder;

namespace Kern.Authorization.Extensions;

public static class EndpointExtension
{
    public static TBuilder WithPermission<TBuilder>(this TBuilder builder, string permission)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.RequireAuthorization($"{permission}_PERMISSION_POLICY");
        return builder;
    }
}