namespace zapURL.Api.Services;

internal sealed class ShortenUrlService : IShortenUrlService
{
    private readonly string _alphaNumericString =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private readonly Random _random = new();

    private readonly Dictionary<string, string> _urls = new();

    public Task<string> GenerateCodeAsync(string url)
    {
        var code = new string(Enumerable.Range(0, 8)
            .Select(_ => _alphaNumericString[_random.Next(_alphaNumericString.Length)])
            .ToArray());

        _urls.Add(code, url);

        return Task.FromResult(code);
    }

    public string GetByCodeAsync(string code)
    {
        _urls.TryGetValue(code, out var url);
        return url ?? string.Empty;
    }
}