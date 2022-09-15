using Kern.Response.Dtos;
using Microsoft.AspNetCore.Http;

namespace Kern.Response;

public static class JsonResponse
{
    private const string SuccessMessage = "Success";
    private const string ConflictMessage = "Conflict";
    private const string BadRequestMessage = "Bad request";
    private const string UnauthorizedMessage = "Unauthorized";
    private const string ForbiddenMessage = "Forbidden";
    private const string NotfoundMessage = "Not found";
    private const string ServerErrorMessage = "Server error";

    /// <summary>Show success response.</summary>
    /// <param name="message">Response message.</param>
    /// <returns>Http response.</returns>
    public static IResult Success(string? message = null)
    {
        return Results.Ok(new ResponseDto { Message = message ?? SuccessMessage });
    }

    /// <summary>Show success response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="data">Response data.</param>
    /// <returns>Http response.</returns>
    public static IResult Success<T>(string? message = null, T? data = null) where T : class
    {
        return data == null
            ? Results.Ok(new ResponseDto { Message = message ?? SuccessMessage })
            : Results.Ok(new ResponseDataDto<T>
            {
                Message = message ?? SuccessMessage,
                Data = data
            });
    }

    /// <summary>Show conflict response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult Conflict(string? message = null, object? errors = null)
    {
        return errors == null
            ? Results.Conflict(new ResponseDto { Message = message ?? ConflictMessage })
            : Results.Conflict(new ResponseErrorDto
            {
                Message = message ?? ConflictMessage,
                Errors = errors
            });
    }

    /// <summary>Show bad request response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult BadRequest(string? message = null, object? errors = null)
    {
        return errors == null
            ? Results.BadRequest(new ResponseDto { Message = message ?? BadRequestMessage })
            : Results.BadRequest(new ResponseErrorDto
            {
                Message = message ?? BadRequestMessage,
                Errors = errors
            });
    }

    /// <summary>Show unauthorized response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult Unauthorized(string? message = null, object? errors = null)
    {
        return Results.Json(errors == null
            ? new ResponseDto { Message = message ?? UnauthorizedMessage }
            : new ResponseErrorDto
            {
                Message = message ?? UnauthorizedMessage,
                Errors = errors
            }, statusCode: 401);
    }

    /// <summary>Show forbidden response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult Forbidden(string? message = null, object? errors = null)
    {
        return Results.Json(errors == null
            ? new ResponseDto { Message = message ?? ForbiddenMessage }
            : new ResponseErrorDto
            {
                Message = message ?? ForbiddenMessage,
                Errors = errors
            }, statusCode: 403);
    }

    /// <summary>Show not found response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult NotFound(string? message = null, object? errors = null)
    {
        return errors == null
            ? Results.NotFound(new ResponseDto { Message = message ?? NotfoundMessage })
            : Results.NotFound(new ResponseErrorDto
            {
                Message = message ?? NotfoundMessage,
                Errors = errors
            });
    }

    /// <summary>Show server error response.</summary>
    /// <param name="message">Response message.</param>
    /// <param name="errors">Response errors.</param>
    /// <returns>Http response.</returns>
    public static IResult ServerError(string? message = null, object? errors = null)
    {
        return Results.Json(errors == null
            ? new ResponseDto { Message = message ?? ServerErrorMessage }
            : new ResponseErrorDto
            {
                Message = message ?? ServerErrorMessage,
                Errors = errors
            }, statusCode: 500);
    }
}