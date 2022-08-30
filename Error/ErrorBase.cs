namespace Kern.Internal.Error;

public class ErrorBase
{
    public ErrorBase()
    {
    }

    public ErrorBase(string message)
    {
        Message = message;
    }

    public virtual string? Message { get; }
}