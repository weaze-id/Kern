namespace Kern.Internal.Response.Dtos;

public class ResponseErrorDto
{
    public string? Message { get; set; }
    public object? Errors { get; set; }
}