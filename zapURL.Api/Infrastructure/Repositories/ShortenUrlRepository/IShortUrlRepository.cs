using ErrorOr;
using zapURL.Api.Models;

namespace zapURL.Api.Infrastructure.Repositories.ShortenUrlRepository;

internal interface IShortUrlRepository
{
    Task<ErrorOr<string>> AddShortUrlAsync(ShortUrl shortUrl);
    Task<ErrorOr<string>> GetOriginalUrlAsync(string code);
    Task<ErrorOr<List<ShortUrl>>> GetAllShortUrlsAsync();
    Task<ErrorOr<string>> UpdateUrlAsync(Guid id, string originalUrl);
    Task<ErrorOr<string>> DeleteAsync(Guid id);
}