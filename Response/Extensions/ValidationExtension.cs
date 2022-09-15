using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Kern.Response.Extensions;

public static class ValidationExtension
{
    public static IResult Response(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );

        return JsonResponse.BadRequest("One or more validation errors occurred", errors);
    }
}