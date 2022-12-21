using Kern.Response.Dtos;
using Kern.Response.Models;
using Microsoft.AspNetCore.Http;

namespace Kern.Response;

public static class JsonResponse
{
    private static readonly ResponseModel SuccessResponse = new()
    {
        Type = "https://tools.ietf.org/html/rfc7231#section-6.3.1",
        Title = "Success"
    };

    private static readonly ResponseModel BadRequestResponse = new()
    {
        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        Title = "Bad Request"
    };

    private static readonly ResponseModel UnauthorizedResponse = new()
    {
        Type = "https://tools.ietf.org/rfc/rfc7235#section-3.1",
        Title = "Unauthorized"
    };

    private static readonly ResponseModel ForbiddenResponse = new()
    {
        Type = "https://tools.ietf.org/rfc/rfc7231#section-6.5.3",
        Title = "Forbidden"
    };

    private static readonly ResponseModel NotFoundResponse = new()
    {
        Type = "https://tools.ietf.org/rfc/rfc7231#section-6.5.4",
        Title = "Bad Found"
    };

    private static readonly ResponseModel ConflictResponse = new()
    {
        Type = "https://tools.ietf.org/rfc/rfc7231#section-6.5.8",
        Title = "Conflict"
    };

    private static readonly ResponseModel ServerErrorResponse = new()
    {
        Type = "https://tools/ietf.org/rfc/rfc7231#section-6.6.1",
        Title = "Internal Server Error"
    };

    private static readonly ResponseModel ServiceUnvailableResponse = new()
    {
        Type = "https://tools/ietf.org/rfc/rfc7231#section-6.6.4",
        Title = "Service Unavailable"
    };

    /// <summary>Show success response.</summary>
    /// <param name="message">Response message.</param>
    /// <returns>Http response.</returns>
    public static IResult Success(string? message = null)
    {
        return Results.Ok(new ResponseDto
        {
            Type = SuccessResponse.Type,
            Title = message ?? SuccessResponse.Title,
            Status = 200
        });
    }

    /// <summary>Show success response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="data">Response data.</param>
    /// <returns>Http response.</returns>
    public static IResult Success<T>(string? message = null, T? data = null) where T : class
    {
        return data == null
            ? Results.Ok(new ResponseDto
            {
                Type = SuccessResponse.Type,
                Title = message ?? SuccessResponse.Title,
                Status = 200
            })
            : Results.Ok(new ResponseDataDto<T>
            {
                Type = SuccessResponse.Type,
                Title = message ?? SuccessResponse.Title,
                Status = 200,
                Data = data
            });
    }

    /// <summary>Show conflict response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult Conflict(string? message = null)
    {
        return Results.Conflict(new ResponseDto
        {
            Type = ConflictResponse.Type,
            Title = message ?? ConflictResponse.Title,
            Status = 409
        });
    }

    /// <summary>Show bad request response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult BadRequest(string? message = null)
    {
        return Results.BadRequest(new ResponseDto
        {
            Type = BadRequestResponse.Type,
            Title = message ?? BadRequestResponse.Title,
            Status = 400
        });
    }

    /// <summary>Show unauthorized response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult Unauthorized(string? message = null)
    {
        return Results.Json(new ResponseDto
        {
            Type = UnauthorizedResponse.Type,
            Title = message ?? UnauthorizedResponse.Title,
            Status = 401
        }, statusCode: 401);
    }

    /// <summary>Show forbidden response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult Forbidden(string? message = null)
    {
        return Results.Json(new ResponseDto
        {
            Type = ForbiddenResponse.Type,
            Title = message ?? ForbiddenResponse.Title,
            Status = 403
        }, statusCode: 403);
    }

    /// <summary>Show not found response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult NotFound(string? message = null)
    {
        return Results.NotFound(new ResponseDto
        {
            Type = NotFoundResponse.Type,
            Title = message ?? NotFoundResponse.Title,
            Status = 404
        });
    }

    /// <summary>Show server error response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult ServerError(string? message = null)
    {
        return Results.Json(new ResponseDto
        {
            Type = ServerErrorResponse.Type,
            Title = message ?? ServerErrorResponse.Title,
            Status = 500
        }, statusCode: 500);
    }

    /// <summary>Show service unavailable response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult ServiceUnavailable(string? message = null)
    {
        return Results.Json(new ResponseDto
        {
            Type = ServiceUnvailableResponse.Type,
            Title = message ?? ServiceUnvailableResponse.Title,
            Status = 503
        }, statusCode: 503);
    }
}