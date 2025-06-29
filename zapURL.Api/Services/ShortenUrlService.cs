using zapURL.Api.Utilities;

namespace zapURL.Api.Services;

internal sealed class ShortenUrlService : IShortenUrlService
{
    private readonly Dictionary<string, string> _urls = new();

    public Task<string> ShortenUrlAsync(string url)
    {
        var code = CodeGenerator.GenerateCode();

        _urls.Add(code, url);

        return Task.FromResult(code);
    }

    public string GetByCodeAsync(string code)
    {
        _urls.TryGetValue(code, out var url);
        return url ?? string.Empty;
    }
}