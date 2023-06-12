using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kern.AspNetCore.Extensions;

public static class WebApplicationExtensions
{
    public static IEndpointRouteBuilder UseSwaggerDocs(
        this IEndpointRouteBuilder route,
        IApplicationBuilder app,
        IHostEnvironment environment)
    {
        var wwwrootPath = Path.Combine(environment.ContentRootPath, "wwwroot");

        app.UseOutputCache();

        route.Map("/docs/swagger.json", async () =>
        {
            var content = await File.ReadAllTextAsync(Path.Combine(wwwrootPath, "json", "swagger.json"));
            return Results.Content(content, "application/json", Encoding.UTF8);
        }).CacheOutput();

        route.Map("/docs", async () =>
        {
            var content = await File.ReadAllTextAsync(Path.Combine(wwwrootPath, "html", "swagger.html"));
            return Results.Content(content, "text/html", Encoding.UTF8);
        }).CacheOutput();

        return route;
    }
}