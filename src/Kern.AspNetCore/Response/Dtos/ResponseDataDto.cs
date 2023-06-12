namespace Kern.Response.Dtos;

public class ResponseDataDto<T>
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public T? Data { get; set; }
}