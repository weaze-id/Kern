using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Kern.Controller;

public static class ServiceCollectionExtensions
{
    private static readonly List<IController> controllers = new();

    public static IServiceCollection AddController<T>(this IServiceCollection services) where T : IController
    {
        var controller = Activator.CreateInstance<T>();

        controller.AddServices(services);
        controllers.Add(controller);

        return services;
    }

    public static WebApplication MapControllers(this WebApplication app)
    {
        for (var i = 0; i < controllers.Count; i++)
        {
            var controller = controllers[i];
            controller.MapEndpoints(app);
        }

        return app;
    }
}