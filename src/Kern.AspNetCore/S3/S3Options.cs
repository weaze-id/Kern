namespace Kern.AspNetCore.S3;

public class S3Options
{
    public const string OptionName = "S3";

    public required string Endpoint { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
    public required string BucketName { get; set; }
    public required bool WithSSL { get; set; }
}