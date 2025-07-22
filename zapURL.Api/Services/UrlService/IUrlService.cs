using ErrorOr;
using zapURL.Api.Models;

namespace zapURL.Api.Services.UrlService;

public interface IUrlService
{
    Task<ErrorOr<string>> ShortenUrlAsync(string originalUrl);
    Task<ErrorOr<string>> GetOriginalUrlAsync(string code);
    Task<ErrorOr<List<ShortUrl>>> GetShortUrlsAsync();
    Task<ErrorOr<string>> UpdateUrlAsync(Guid id, string originalUrl);
    Task<ErrorOr<string>> Delete(Guid id);
}