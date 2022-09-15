using System.Security.Cryptography;
using System.Text;

namespace Kern.Utility;

public static class CryptoUtil
{
    /// <summary>Generate random 32 byte base64 string as salt.</summary>
    /// <returns>base64 salt.</returns>
    public static string GenerateSalt()
    {
        var saltBytes = new byte[32];
        RandomNumberGenerator.Fill(saltBytes);

        return Convert.ToBase64String(saltBytes);
    }

    /// <summary>Combine password and salt, then compute the hash.</summary>
    /// <param name="password">User password.</param>
    /// <param name="salt">32 bit salt converted to base64 string.</param>
    /// <returns>Hashed password.</returns>
    public static string HashPassword(string password, string salt)
    {
        // Hash password + salt.
        var saltedPassword = $"{password}{salt}";
        var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);

        using var sha512 = SHA512.Create();
        return Convert.ToBase64String(sha512.ComputeHash(saltedPasswordBytes));
    }
}