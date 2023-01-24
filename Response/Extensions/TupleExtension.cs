using FluentValidation.Results;
using Kern.Error;
using Microsoft.AspNetCore.Http;

namespace Kern.Response.Extensions;

public static class TupleExtension
{
    public static IResult Response<T>(this (T?, ValidationResult?, ErrorBase?) tuple) where T : class
    {
        var (result, validationResult, error) = tuple;
        if (validationResult != null)
        {
            return validationResult.Response();
        }

        if (error != null)
        {
            return error.Response();
        }

        return JsonResponse.Success(data: result);
    }

    public static IResult Response<T>(this (T?, ErrorBase?) tuple) where T : class
    {
        var (result, error) = tuple;
        if (error != null)
        {
            return error.Response();
        }

        return JsonResponse.Success(data: result);
    }

    public static IResult Response(this (ValidationResult?, ErrorBase?) tuple)
    {
        var (validationResult, error) = tuple;
        if (validationResult != null)
        {
            return validationResult.Response();
        }

        if (error != null)
        {
            return error.Response();
        }

        return JsonResponse.Success();
    }
}