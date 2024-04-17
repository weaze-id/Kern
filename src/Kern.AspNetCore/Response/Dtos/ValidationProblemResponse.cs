namespace Kern.Response.Dtos;

public class ValidationProblemResponse
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
}