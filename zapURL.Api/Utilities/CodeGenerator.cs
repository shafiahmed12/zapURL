namespace zapURL.Api.Utilities;

public static class CodeGenerator
{
    private const string AlphaNumericString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private static readonly Random Random = new();

    public static string GenerateCode()
    {
        var code = Enumerable.Range(0, 8).Select(_ => AlphaNumericString[Random.Next(AlphaNumericString.Length)])
            .ToArray();
        return string.Join("", code);
    }
}