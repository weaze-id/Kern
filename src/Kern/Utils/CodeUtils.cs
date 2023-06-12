namespace Kern.Utils;

public static class CodeUtils
{
    private static Random random = new Random();
    private static string numberChars = "0123456789";
    private static string alphabetChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private static string aplhabetAndNumberChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateRandomNumberCode(int codeLength)
    {
        return GenerateRandomCode(numberChars, codeLength);
    }

    public static string GenerateRandomStringCode(int codeLength)
    {
        return GenerateRandomCode(alphabetChars, codeLength);
    }

    public static string GenerateRandomCode(int codeLength)
    {
        return GenerateRandomCode(aplhabetAndNumberChars, codeLength);
    }

    private static string GenerateRandomCode(string chars, int codeLength)
    {
        Span<char> code = new char[codeLength];
        for (int i = 0; i < codeLength; i++)
        {
            code[i] = chars[random.Next(chars.Length)];
        }

        return new string(code);
    }
}