using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Kern.Extensions;

public static class BuilderExtensions
{
    public static WebApplication UseSwaggerDocs(this WebApplication app)
    {
        app.Map("/docs/swagger.json", async () =>
        {
            var content =
                await File.ReadAllTextAsync(Path.Combine(app.Environment.ContentRootPath, "wwwroot", "json",
                    "swagger.json"));
            return Results.Content(content, "application/json", Encoding.UTF8);
        }).CacheOutput();

        app.Map("/docs", async () =>
        {
            var content =
                await File.ReadAllTextAsync(Path.Combine(app.Environment.ContentRootPath, "wwwroot", "html",
                    "swagger.html"));
            return Results.Content(content, "text/html", Encoding.UTF8);
        }).CacheOutput();

        return app;
    }
}