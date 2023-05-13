using Microsoft.AspNetCore.Routing;

namespace Kern.AspNetCore.Endpoints;

public interface IEndpoints
{
    IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
}