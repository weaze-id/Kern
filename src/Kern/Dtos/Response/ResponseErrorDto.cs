namespace Kern.Dtos.Response;

public class ResponseErrorDto
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public object? Errors { get; set; }
}