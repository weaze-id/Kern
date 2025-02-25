using FluentValidation;
using Kern.AspNetCore.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Kern.AspNetCore;

public static class ValidationFilterExtensions
{
    public static RouteHandlerBuilder AddValidationFilter<T>(this RouteHandlerBuilder route) where T : class
    {
        return route.AddEndpointFilter(async (efiContext, next) =>
        {
            if (efiContext.Arguments.SingleOrDefault(e => e?.GetType() == typeof(T)) is not T validateable)
            {
                return JsonResult.BadRequest();
            }

            var validator = efiContext.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();
            var validationResult = await validator.ValidateAsync(validateable);

            if (!validationResult.IsValid)
            {
                return JsonResult.BadRequest(validationResult: validationResult);
            }

            return await next(efiContext);
        });
    }
}