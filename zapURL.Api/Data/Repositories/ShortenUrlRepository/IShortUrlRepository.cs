using zapURL.Api.Models;

namespace zapURL.Api.Data.Repositories.ShortenUrlRepository;

internal interface IShortUrlRepository
{
    Task<bool> AddShortUrlAsync(ShortUrl shortUrl);
    Task<string> GetOriginalUrlAsync(string code);
    Task<List<ShortUrl>> GetAllShortUrlsAsync();
    Task UpdateUrlAsync(Guid id, string originalUrl);
    Task DeleteAsync(Guid id);
}