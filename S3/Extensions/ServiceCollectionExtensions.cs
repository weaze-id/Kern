using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;

namespace Kern.S3.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddS3(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<S3Options>(configuration.GetSection(S3Options.OptionName));
        services.AddScoped(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<S3Options>>();
            var httpClient = serviceProvider.GetRequiredService<HttpClient>()!;

            var optionsValue = options.Value;
            var minioClient = new MinioClient()
                .WithEndpoint(optionsValue.Endpoint)
                .WithCredentials(optionsValue.AccessKey, optionsValue.SecretKey)
                .WithSSL(optionsValue.WithSSL)
                .WithHttpClient(httpClient)
                .Build();

            return new S3(minioClient, options);
        });

        return services;
    }
}