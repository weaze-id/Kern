namespace Kern.Utils;

public static class CodeUtils
{
    public static string GenerateCode()
    {
        var random = new Random();
        var randomNumber = random.Next(10000000, 100000000);
        return randomNumber.ToString();
    }
}