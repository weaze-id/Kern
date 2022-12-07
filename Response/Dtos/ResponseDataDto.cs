namespace Kern.Response.Dtos;

public class ResponseDataDto<T>
{
    public required string Type { get; set; }
    public required string Title { get; set; }
    public required int Status { get; set; }
    public T? Data { get; set; }
}