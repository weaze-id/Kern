namespace Kern.Internal.Authorization.Metadatas;

public class RequireAuthorizationMetadata
{
    public RequireAuthorizationMetadata(string? permission = null)
    {
        Permission = permission;
    }

    public string? Permission { get; }
}