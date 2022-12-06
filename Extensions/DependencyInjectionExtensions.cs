using IdGen;
using Kern.Constants;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Kern.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddIdGenerator(this IServiceCollection services)
    {
        services.AddSingleton(_ =>
        {
            var structure = new IdStructure(45, 6, 12);
            var epoch = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var options = new IdGeneratorOptions(
                structure,
                new DefaultTimeSource(epoch),
                SequenceOverflowStrategy.SpinWait);

            return new IdGenerator(0, options);
        });

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddFluentValidationRulesToSwagger();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(configuration["Swagger:Version"], new OpenApiInfo
            {
                Title = configuration["Swagger:Title"],
                Description = StringConstants.SWAGGER_DESCRIPTION
                    .Replace("\r", "")
                    .Replace("{{Title}}", configuration["Swagger:Title"]),
                Version = configuration["Swagger:Version"]
            });
        });

        return services;
    }
}