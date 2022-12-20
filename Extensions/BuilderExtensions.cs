using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Kern.Extensions;

public static class BuilderExtensions
{
    public static WebApplication UseSwaggerDocs(this WebApplication app)
    {
        app.UseOutputCache();

        app.Map("/docs/swagger.json", async () =>
        {
            using var fileStream =
                File.OpenRead(
                    Path.Combine(app.Environment.ContentRootPath, "wwwroot", "json", "swagger.json"));

            var json = await JsonSerializer.DeserializeAsync<object>(fileStream);
            var jsonString = JsonSerializer.SerializeToUtf8Bytes(json);

            return Results.Content(Encoding.UTF8.GetString(jsonString), "application/json", Encoding.UTF8);
        }).CacheOutput();

        app.Map("/docs", async () =>
        {
            var content =
                await File.ReadAllTextAsync(
                    Path.Combine(app.Environment.ContentRootPath, "wwwroot", "html", "swagger.html"));
            return Results.Content(content, "text/html", Encoding.UTF8);
        }).CacheOutput();

        return app;
    }
}