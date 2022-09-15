namespace Kern.Error;

public class AuthenticationError : ErrorBase
{
    public AuthenticationError()
    {
    }

    public AuthenticationError(string message) : base(message)
    {
    }
}