namespace Kern.Utils;

public static class EnumUtils
{
    public static bool Contains<TEnum>(TEnum valueToCheck) where TEnum : Enum
    {
        return Enum.IsDefined(typeof(TEnum), valueToCheck);
    }

    public static TEnum ToEnum<TEnum>(string stringValue) where TEnum : Enum
    {
        if (Enum.TryParse(typeof(TEnum), stringValue, false, out var result))
        {
            return (TEnum)result;
        }

        return default!;
    }

    public static string Print<TEnum>() where TEnum : Enum
    {
        var names = Enum.GetNames(typeof(TEnum)).Skip(1).ToArray();
        return string.Join(", ", names);
    }
}