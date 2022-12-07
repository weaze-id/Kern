namespace Kern.Response.Dtos;

public class ResponseErrorDto
{
    public required string Type { get; set; }
    public required string Title { get; set; }
    public required int Status { get; set; }
    public required object Errors { get; set; }
}