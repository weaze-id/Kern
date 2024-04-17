using Kern.Error;
using Microsoft.AspNetCore.Http;

namespace Kern.AspNetCore.Response.Extensions;

public static class ErrorExtension
{
    public static IResult Response(this ErrorBase error)
    {
        return error switch
        {
            AuthenticationError => JsonResult.Unauthorized(error.Message),
            AuthorizationError => JsonResult.Forbidden(error.Message),
            BadRequestError => JsonResult.BadRequest(error.Message),
            ConflictError => JsonResult.Conflict(error.Message),
            NotFoundError => JsonResult.NotFound(error.Message),
            ServiceUnavailableError => JsonResult.ServiceUnavailable(error.Message),
            _ => JsonResult.ServerError(error.Message)
        };
    }
}