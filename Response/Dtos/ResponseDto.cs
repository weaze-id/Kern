namespace Kern.Response.Dtos;

public class ResponseDto
{
    public required string Type { get; set; }
    public required string Title { get; set; }
    public required int Status { get; set; }
}