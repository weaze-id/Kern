namespace Kern.Response.Dtos;

public class ResponseDataDto<T>
{
    public string? Message { get; set; }
    public T? Data { get; set; }
}