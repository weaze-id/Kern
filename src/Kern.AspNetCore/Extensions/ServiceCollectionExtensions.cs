using Kern.AspNetCore.Constants;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Kern.AspNetCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOutputCache();
        services.AddEndpointsApiExplorer();
        services.AddFluentValidationRulesToSwagger();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(configuration["Swagger:Version"], new OpenApiInfo
            {
                Title = configuration["Swagger:Title"],
                Description = StringConstants.SWAGGER_DESCRIPTION
                    .Replace("\r", "")
                    .Replace("{{Title}}", configuration["Swagger:Title"])
                    .Replace("{{Version}}", configuration["Swagger:Version"]),
                Version = configuration["Swagger:Version"]
            });
        });

        return services;
    }
}