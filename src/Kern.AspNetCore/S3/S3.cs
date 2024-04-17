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
    /// Retrieves an object from the specified bucket asynchronously.
    /// </summary>
    /// <param name="objectName">The name of the object to retrieve.</param>
    /// <param name="offset">Optional. The starting offset of the object to retrieve. Default is null.</param>
    /// <param name="length">Optional. The length of the object to retrieve. Default is null.</param>
    /// <returns>
    /// A <see cref="Stream"/> containing the retrieved object.
    /// </returns>
    public async Task<Stream?> GetObjectAsync(
        string objectName,
        long? offset = null,
        long? length = null)
    {
        // Create a memory stream to store the object data
        var memoryStream = new MemoryStream();

        try
        {
            // Prepare arguments to check if the object exists
            var statObjectArgs = new StatObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(objectName);

            // Check if the object exists
            await _minioClient.StatObjectAsync(statObjectArgs);

            // Prepare arguments to get the object and copy it to the memory stream
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(objectName)
                .WithCallbackStream(async (stream) => await stream.CopyToAsync(memoryStream));

            // If offset and length are specified, set them in the arguments
            if (offset != null && length != null)
            {
                getObjectArgs.WithOffsetAndLength(
                    offset.GetValueOrDefault(),
                    length.GetValueOrDefault());
            }

            // Get the object asynchronously and copy it to the memory stream
            await _minioClient.GetObjectAsync(getObjectArgs).ConfigureAwait(false);

            // Return the memory stream containing the retrieved object data
            return memoryStream;
        }
        catch
        {
            // Dispose the memory stream.
            await memoryStream.DisposeAsync();

            // If an exception occurs, return null
            return null;
        }
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