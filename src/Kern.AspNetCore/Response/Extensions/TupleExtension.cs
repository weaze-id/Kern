using FluentValidation.Results;
using Kern.Error;
using Microsoft.AspNetCore.Http;

namespace Kern.AspNetCore.Response.Extensions;

public static class TupleExtension
{
    public static IResult Response<T>(this (T?, ValidationResult?, ErrorBase?) tuple) where T : class
    {
        var (result, validationResult, error) = tuple;
        if (validationResult != null)
        {
            return JsonResult.BadRequest(validationResult: validationResult);
        }

        if (error != null)
        {
            return error.Response();
        }

        return JsonResult.Success(data: result);
    }

    public static IResult Response<T>(this (T?, ErrorBase?) tuple) where T : class
    {
        var (result, error) = tuple;
        if (error != null)
        {
            return error.Response();
        }

        return JsonResult.Success(data: result);
    }

    public static IResult Response(this (ValidationResult?, ErrorBase?) tuple)
    {
        var (validationResult, error) = tuple;
        if (validationResult != null)
        {
            return JsonResult.BadRequest(validationResult: validationResult);
        }

        if (error != null)
        {
            return error.Response();
        }

        return JsonResult.Success();
    }
}