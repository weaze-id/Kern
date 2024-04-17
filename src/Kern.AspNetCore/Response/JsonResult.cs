using FluentValidation.Results;
using Kern.AspNetCore.Response.Models;
using Kern.Response.Dtos;
using Microsoft.AspNetCore.Http;

namespace Kern.AspNetCore.Response;

public static class JsonResult
{
    private static readonly Dictionary<int, ResponseDetail> responseModels = new()
    {
        [200] = new ResponseDetail
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.3.1",
            Title = "Success",
            Status = 200
        },
        [400] = new ResponseDetail
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Bad Request",
            Status = 400
        },
        [401] = new ResponseDetail
        {
            Type = "https://tools.ietf.org/rfc/rfc7235#section-3.1",
            Title = "Unauthorized",
            Status = 401
        },
        [403] = new ResponseDetail
        {
            Type = "https://tools.ietf.org/rfc/rfc7231#section-6.5.3",
            Title = "Forbidden",
            Status = 403
        },
        [404] = new ResponseDetail
        {
            Type = "https://tools.ietf.org/rfc/rfc7231#section-6.5.4",
            Title = "Not Found",
            Status = 404
        },
        [409] = new ResponseDetail
        {
            Type = "https://tools.ietf.org/rfc/rfc7231#section-6.5.8",
            Title = "Conflict",
            Status = 409
        },
        [500] = new ResponseDetail
        {
            Type = "https://tools/ietf.org/rfc/rfc7231#section-6.6.1",
            Title = "Internal Server Error",
            Status = 500
        },
        [503] = new ResponseDetail
        {
            Type = "https://tools/ietf.org/rfc/rfc7231#section-6.6.4",
            Title = "Service Unavailable",
            Status = 503
        }
    };

    /// <summary>Show success response.</summary>
    /// <param name="message">Response message.</param>
    /// <returns>Http response.</returns>
    public static IResult Success(string? message = null)
    {
        return Results.Ok(new CommonResponse
        {
            Type = responseModels[200].Type,
            Title = message ?? responseModels[200].Title,
            Status = responseModels[200].Status
        });
    }

    /// <summary>Show success response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="data">Response data.</param>
    /// <returns>Http response.</returns>
    public static IResult Success<T>(string? message = null, T? data = null) where T : class
    {
        return data == null
            ? Results.Ok(new CommonResponse
            {
                Type = responseModels[200].Type,
                Title = message ?? responseModels[200].Title,
                Status = responseModels[200].Status
            })
            : Results.Ok(new DataResponse<T>
            {
                Type = responseModels[200].Type,
                Title = message ?? responseModels[200].Title,
                Status = responseModels[200].Status,
                Data = data
            });
    }

    /// <summary>Show bad request response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult BadRequest(string? message = null, ValidationResult? validationResult = null)
    {
        return validationResult == null
            ? Results.BadRequest(new CommonResponse
            {
                Type = responseModels[400].Type,
                Title = message ?? responseModels[400].Title,
                Status = 400
            })
            : Results.BadRequest(new ValidationProblemResponse
            {
                Type = responseModels[400].Type,
                Title = message ?? responseModels[400].Title,
                Status = 400,
                Errors = validationResult.ToDictionary(),
            });
    }

    /// <summary>Show unauthorized response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult Unauthorized(string? message = null)
    {
        return Results.Json(new CommonResponse
        {
            Type = responseModels[401].Type,
            Title = message ?? responseModels[401].Title,
            Status = responseModels[401].Status
        }, statusCode: responseModels[401].Status);
    }

    /// <summary>Show forbidden response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult Forbidden(string? message = null)
    {
        return Results.Json(new CommonResponse
        {
            Type = responseModels[403].Type,
            Title = message ?? responseModels[403].Title,
            Status = responseModels[403].Status
        }, statusCode: responseModels[403].Status);
    }

    /// <summary>Show not found response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult NotFound(string? message = null)
    {
        return Results.NotFound(new CommonResponse
        {
            Type = responseModels[404].Type,
            Title = message ?? responseModels[404].Title,
            Status = responseModels[404].Status
        });
    }

    /// <summary>Show conflict response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult Conflict(string? message = null)
    {
        return Results.Conflict(new CommonResponse
        {
            Type = responseModels[409].Type,
            Title = message ?? responseModels[409].Title,
            Status = responseModels[409].Status
        });
    }

    /// <summary>Show server error response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult ServerError(string? message = null)
    {
        return Results.Json(new CommonResponse
        {
            Type = responseModels[500].Type,
            Title = message ?? responseModels[500].Title,
            Status = responseModels[500].Status
        }, statusCode: responseModels[500].Status);
    }

    /// <summary>Show service unavailable response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult ServiceUnavailable(string? message = null)
    {
        return Results.Json(new CommonResponse
        {
            Type = responseModels[503].Type,
            Title = message ?? responseModels[503].Title,
            Status = responseModels[503].Status
        }, statusCode: responseModels[503].Status);
    }
}