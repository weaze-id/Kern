using Microsoft.AspNetCore.Http;

namespace Kern.Internal.Utility;

public class PathUtil
{
    public static string MediaFilePath(HttpContext httpContext, string fileName)
    {
        var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}";
        return $"{baseUrl}/media/{fileName}";
    }
}