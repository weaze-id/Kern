using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Kern.Services;

public static class SwaggerService
{
    private const string DESCRIPTION =
        @"# Introduction
The {{Title}} API follows the general patterns of REST.
You can use the resources of a {{Title}} account (items, receipts, etc.) 
by making HTTPS requests to URLs that represent those resources. You can find description of all the endpoints here.

# Getting Started
To test and explore {{Title}} API you can use Postman.
Postman is a powerful HTTP client for testing RESTful APIs by displaying requests and responses in manageable formats.

These are steps you need to take to start testing {{Title}} API via Postman:
- Download and install Postman. You can get it here: [https://www.getpostman.com](https://www.getpostman.com)
- Get [{{Title}} API Postman Collection](/swagger/v1/swagger.json) and import it into Postman.
- Define variables used in postman collection. 
    For example, in {{Title}} production environment for {{Title}} API v1.0 you should define baseUrl variable.
    It is useful to configure variables in postman environments so you will not have to redefine the values for each request manually.
    [Learn more about Postman environments](https://learning.postman.com/docs/sending-requests/managing-environments/).
- Configure the Postman Authorization header. Each request to {{Title}} API should contain a token in request header.
    You can read more about ways to get the token in the Authorization section of the documentation.
- Once you complete these steps you are ready to make calls to {{Title}} API via Postman.

# Authorization
An application credential is any piece of information that identifies, authenticates, or authorizes an application in some way.

{{Title}} API provide one authorization method:
- Bearer token

## Bearer token
Bearer tokens are a simple way to make calls to the API.
You can get your bearer token by hitting login endpoint.
For every call to the API you must include your access token in the Authorization header:

```
    Authorization: Bearer eyJhbGciOiJIUzI1NiI...
```
";

    public static IServiceCollection AddSwagger(this IServiceCollection services, string title, string version)
    {
        services.AddEndpointsApiExplorer();
        services.AddFluentValidationRulesToSwagger();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(version, new OpenApiInfo
            {
                Title = title,
                Description = DESCRIPTION
                    .Replace("\r", "")
                    .Replace("{{Title}}", title),
                Version = version
            });
        });

        return services;
    }

    public static WebApplication UseSwagger(this WebApplication app, string title)
    {
        app.UseSwagger();
        app.UseReDoc(c =>
        {
            c.RoutePrefix = "docs";
            c.DocumentTitle = @$"{title} Docs";
        });

        return app;
    }
}