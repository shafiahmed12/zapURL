namespace zapURL.Api.Services;

public interface IShortenUrlService
{
    Task<string> GenerateCodeAsync(string url);

    string GetByCodeAsync(string code);
}