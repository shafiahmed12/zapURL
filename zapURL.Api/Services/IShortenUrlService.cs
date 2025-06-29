namespace zapURL.Api.Services;

public interface IShortenUrlService
{
    Task<string> ShortenUrlAsync(string url);

    string GetByCodeAsync(string code);
}