namespace Kern.Internal.Module;

public interface IModule
{
    IServiceCollection RegisterModule(IServiceCollection builder);
    IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
}