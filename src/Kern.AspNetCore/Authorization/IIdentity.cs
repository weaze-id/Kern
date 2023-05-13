using System.Security.Claims;

namespace Kern.AspNetCore.Authorization;

public interface IIdentity
{
    /// <summary>Convert IIdentity to list of claims.</summary>
    /// <returns>A list of claims.</returns>
    public List<Claim> ToClaims();
}