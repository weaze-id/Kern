namespace Kern.Error;

public class AuthorizationError : ErrorBase
{
    public AuthorizationError()
    {
    }

    public AuthorizationError(string message) : base(message)
    {
    }
}