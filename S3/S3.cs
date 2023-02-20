using Microsoft.Extensions.Options;
using Minio;

namespace Kern.S3;

public class S3
{
    private readonly MinioClient _minioClient;
    private readonly S3Options _options;

    public S3(MinioClient minioClient, IOptions<S3Options> options)
    {
        _minioClient = minioClient;
        _options = options.Value;
    }

    public string GenerateUrl(string objectName)
    {
        return
            $"{(_options.WithSSL ? "https" : "http")}://{_options.Endpoint}/{_options.BucketName}/{Path.Combine(_options.Directory, objectName)}";
    }

    public async Task StoreObject(
        string objectName,
        Stream stream,
        string contentType,
        Dictionary<string, string>? metadata = null)
    {
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(Path.Combine(_options.Directory, objectName))
            .WithObjectSize(stream.Length)
            .WithStreamData(stream)
            .WithContentType(contentType);

        if (metadata != null)
        {
            putObjectArgs.WithHeaders(metadata);
        }

        await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
    }
}