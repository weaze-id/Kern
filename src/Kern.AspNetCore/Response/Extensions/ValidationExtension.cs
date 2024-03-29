using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Kern.AspNetCore.Response.Extensions;

public static class ValidationExtension
{
    public static IResult Response(this ValidationResult validationResult)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
}