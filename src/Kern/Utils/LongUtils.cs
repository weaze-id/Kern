namespace Kern.Utils;

public static class LongUtils
{
    public static long? Parse(string? value)
    {
        if (long.TryParse(value, out var longValue))
        {
            return longValue;
        }

        return null;
    }
}