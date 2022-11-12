using Kern.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Sentry;

namespace Kern.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _environtment;

    public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environtment)
    {
        _next = next;
        _environtment = environtment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_environtment.IsDevelopment())
        {
            await _next.Invoke(context);
            return;
        }

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
                    break;
                default:
                    response = JsonResponse.ServerError();
                    SentrySdk.CaptureException(e);

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