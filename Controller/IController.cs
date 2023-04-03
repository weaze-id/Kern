using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Kern.Controller;

public interface IController
{
    IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
}