namespace Kern.Error;

public class ConflictError : ErrorBase
{
    public ConflictError()
    {
    }

    public ConflictError(string message) : base(message)
    {
    }
}