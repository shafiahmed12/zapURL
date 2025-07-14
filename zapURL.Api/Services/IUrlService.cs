using zapURL.Api.Models;

namespace zapURL.Api.Services;

public interface IUrlService
{
    Task<string> ShortenUrlAsync(string originalUrl);
    Task<string> GetOriginalUrlAsync(string code);
    Task<List<ShortUrl>> GetShortUrlsAsync();
    Task UpdateUrlAsync(Guid id, string originalUrl);
    Task Delete(Guid id);
}