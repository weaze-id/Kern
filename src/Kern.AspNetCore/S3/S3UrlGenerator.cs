using System.Buffers;
using Microsoft.Extensions.Options;

namespace Kern.AspNetCore.S3;

public class S3UrlGenerator
{
    private readonly S3Options _options;
    private readonly string _urlPrefix;

    public S3UrlGenerator(IOptions<S3Options> options)
    {
        _options = options.Value;
        _urlPrefix = $"{(_options.WithSSL ? "https" : "http")}://{_options.Endpoint}/{_options.BucketName}/";
    }

    /// <summary>
    /// Generates a complete URL for the specified object name.
    /// </summary>
    /// <param name="objectName">The object name to include in the URL.</param>
    /// <returns>The generated URL.</returns>
    public string GenerateUrl(string objectName)
    {
        // Calculate the lengths and total length of the URL
        var prefixLength = _urlPrefix.Length;
        var objectNameLength = objectName.Length;
        var totalLength = prefixLength + objectNameLength;

        // Rent an array of characters from the shared pool
        var buffer = ArrayPool<char>.Shared.Rent(totalLength);

        // Create a span for the URL buffer
        var urlSpan = buffer.AsSpan(0, totalLength);

        // Copy the URL prefix and object name to the URL span
        _urlPrefix.CopyTo(urlSpan);
        objectName.AsSpan().CopyTo(urlSpan.Slice(prefixLength));

        // Create a new string from the URL span
        var url = new string(urlSpan);

        // Return the rented buffer to the shared pool
        ArrayPool<char>.Shared.Return(buffer);

        return url;
    }
}