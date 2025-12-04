using System.Security.Cryptography;
using System.Text;
using IMS.BLL.Services.Interfaces;

namespace IMS.BLL.Services;

public static class PasswordGenerator
{
    private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string DigitChars = "0123456789";
    private const string SymbolChars = "!@#$%^&*()_+-=[]{}|;':\",./<>?"; 
    private const string AllChars = LowercaseChars + UppercaseChars + DigitChars + SymbolChars;

    private const int MinLength = 16; 

    public static string GenerateRandomPassword()
    {
        var password = new StringBuilder(MinLength);
        
        password.Append(GetRandomChar(LowercaseChars));
        password.Append(GetRandomChar(UppercaseChars));
        password.Append(GetRandomChar(DigitChars));
        password.Append(GetRandomChar(SymbolChars));

        for (var i = password.Length; i < MinLength; i++)
        {
            password.Append(GetRandomChar(AllChars));
        }

        return new string(password.ToString().OrderBy(c => RandomNumberGenerator.GetInt32(256)).ToArray());
    }

    private static char GetRandomChar(string charSet)
    {
        var index = RandomNumberGenerator.GetInt32(charSet.Length);
        return charSet[index];
    }
}
