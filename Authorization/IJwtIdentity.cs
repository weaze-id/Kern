using System.Security.Claims;

namespace Kern.Internal.Authorization;

public interface IJwtIdentity
{
    /// <summary>Convert IJwtIdentity to list of claims.</summary>
    /// <returns>A list of claims.</returns>
    public List<Claim> ToClaims();
}