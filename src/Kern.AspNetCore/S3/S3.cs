using Microsoft.Extensions.Options;
using Minio;

namespace Kern.AspNetCore.S3;

public class S3
{
    public static Dictionary<string, string> PublicReadonlyMetadata = new() { { "x-amz-acl", "public-read" } };

    private readonly MinioClient _minioClient;
    private readonly S3Options _options;

    public S3(MinioClient minioClient, IOptions<S3Options> options)
    {
        _minioClient = minioClient;
        _options = options.Value;
    }

    /// <summary>
    /// Stores an object in the S3 bucket asynchronously.
    /// </summary>
    /// <param name="objectName">The name of the object to store.</param>
    /// <param name="stream">The stream containing the object data.</param>
    /// <param name="contentType">The MIME type of the object.</param>
    /// <param name="metadata">Optional metadata associated with the object.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task StoreObjectAsync(
        string objectName,
        Stream stream,
        string contentType,
        Dictionary<string, string>? metadata = null)
    {
        // Prepare the arguments for putting the object into S3
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_options.BucketName) // Specify the bucket name
            .WithObject(objectName) // Specify the object name
            .WithObjectSize(stream.Length) // Specify the size of the object
            .WithStreamData(stream) // Specify the stream containing the object data
            .WithContentType(contentType); // Specify the MIME type of the object

        if (metadata != null)
        {
            putObjectArgs.WithHeaders(metadata); // Add optional metadata to the object
        }

        // Put the object into S3 asynchronously
        await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes an object from the specified bucket asynchronously.
    /// </summary>
    /// <param name="objectName">The name of the object to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteObjectAsync(string objectName)
    {
        // Prepare the arguments for removing the object from S3
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(_options.BucketName) // Specify the bucket name
            .WithObject(objectName); // Specify the object name

        // Remove the object from S3 asynchronously
        await _minioClient.RemoveObjectAsync(removeObjectArgs).ConfigureAwait(false);
    }
}