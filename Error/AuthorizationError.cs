namespace Kern.Internal.Error;

public class AuthorizationError : ErrorBase
{
    public AuthorizationError()
    {
    }

    public AuthorizationError(string message) : base(message)
    {
    }
}