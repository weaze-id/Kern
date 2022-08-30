namespace Kern.Internal.Error;

public class BadRequestError : ErrorBase
{
    public BadRequestError()
    {
    }

    public BadRequestError(string message) : base(message)
    {
    }
}