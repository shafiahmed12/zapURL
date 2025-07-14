using System.Security.Cryptography;
using System.Text;

namespace zapURL.Api.Utilities;

public static class CodeGenerator
{
    private const string CodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const int CodeLength = 8;

    internal static string GenerateCode()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(CodeLength);
        var code = new StringBuilder("");
        foreach (var randomByte in randomBytes) code.Append(CodeChars[randomByte % CodeChars.Length]);
        return code.ToString();
    }
}