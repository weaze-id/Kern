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

    public string? GenerateUrl(string objectName)
    {
        return $"{(_options.WithSSL ? "https" : "http")}://{_options.Endpoint}/{_options.BucketName}/{objectName}";
    }

    public async Task StoreObject(string objectName, Stream stream, string contentType)
    {
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
    }
}