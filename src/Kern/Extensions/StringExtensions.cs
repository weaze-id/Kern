using System.Text.RegularExpressions;

namespace Kern.Extensions;

public static class StringExtensions
{
    public static string? EmptyToNull(this string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public static string? RemoveWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? value : Regex.Replace(value, @"\s+", string.Empty);
    }
}