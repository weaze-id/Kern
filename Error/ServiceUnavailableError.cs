namespace Kern.Error;

public class ServiceUnavailableError : ErrorBase
{
    public ServiceUnavailableError()
    {
    }

    public ServiceUnavailableError(string message) : base(message)
    {
    }
}