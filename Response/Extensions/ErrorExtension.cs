using Kern.Error;
using Microsoft.AspNetCore.Http;

namespace Kern.Response.Extensions;

public static class ErrorExtension
{
    public static IResult Response(this ErrorBase error)
    {
        return error switch
        {
            AuthenticationError => JsonResponse.Unauthorized(error.Message),
            AuthorizationError => JsonResponse.Forbidden(error.Message),
            BadRequestError => JsonResponse.BadRequest(error.Message),
            ConflictError => JsonResponse.Conflict(error.Message),
            NotFoundError => JsonResponse.NotFound(error.Message),
            ServiceUnavailableError => JsonResponse.ServiceUnavailable(error.Message),
            _ => JsonResponse.ServerError(error.Message)
        };
    }
}