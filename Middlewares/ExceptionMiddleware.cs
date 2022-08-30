using Kern.Internal.Response;
using ILogger = Serilog.ILogger;

namespace Kern.Internal.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _serilogger;

    public ExceptionMiddleware(RequestDelegate next, ILogger serilogger)
    {
        _next = next;
        _serilogger = serilogger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception e)
        {
            IResult? response = null;
            switch (e)
            {
                case BadHttpRequestException:
                    response = JsonResponse.BadRequest();
                    _serilogger.Information("{Message}", e, DateTime.Now);
                    break;
                default:
                    response = JsonResponse.ServerError();
                    _serilogger.Error("{Message}", e, DateTime.Now);
                    break;
            }

            await response.ExecuteAsync(context);
        }
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}