using Kern.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Kern.Authorization.Handlers;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userPermissions = context.User.FindFirst(e => e.Type == "Permissions")?.Value;
        if (userPermissions == null)
        {
            return Task.CompletedTask;
        }

        var permissions = userPermissions.Split(",");
        if (!permissions.Contains(requirement.Permission))
        {
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}