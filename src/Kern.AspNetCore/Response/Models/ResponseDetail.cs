namespace Kern.AspNetCore.Response.Models;

internal class ResponseDetail
{
    public required string Type { get; set; }
    public required string Title { get; set; }
    public required int Status { get; set; }
}