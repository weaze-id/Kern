using System.Security.Cryptography;

namespace Kern.Utils;

public static class CodeUtils
{
    private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
    private static readonly string numberChars = "0123456789";
    private static readonly string alphabetChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private static readonly string aplhabetAndNumberChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

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
        Span<byte> data = stackalloc byte[codeLength];
        rng.GetBytes(data);

        Span<char> code = stackalloc char[codeLength];
        for (var i = 0; i < codeLength; i++)
        {
            var index = data[i] % chars.Length;
            code[i] = chars[index];
        }

        return new string(code);
    }
}