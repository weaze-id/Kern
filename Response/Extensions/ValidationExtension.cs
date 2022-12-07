using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Kern.Response.Extensions;

public static class ValidationExtension
{
    public static IResult Response(this ValidationResult validationResult)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
}