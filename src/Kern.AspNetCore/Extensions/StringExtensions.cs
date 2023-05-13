namespace Kern.AspNetCore.Extensions;

public static class StringExtensions
{
    public static string? EmptyToNull(this string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}