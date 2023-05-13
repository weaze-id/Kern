namespace Kern.Extensions;

public static class DateTimeExtension
{
    public static DateTimeOffset? ToDateTimeOffset(this DateTime? dateTime)
    {
        if (dateTime == null)
        {
            return null;
        }

        return new DateTimeOffset(dateTime.GetValueOrDefault(), TimeSpan.Zero);
    }

    public static long? ToUnixTimeMilliseconds(this DateTime? dateTime)
    {
        var dateTimeOffset = dateTime.ToDateTimeOffset();
        if (dateTimeOffset == null)
        {
            return null;
        }

        return dateTimeOffset?.ToUnixTimeMilliseconds();
    }
}