namespace Kern.Error;

public class NotFoundError : ErrorBase
{
    public NotFoundError()
    {
    }

    public NotFoundError(string message) : base(message)
    {
    }
}