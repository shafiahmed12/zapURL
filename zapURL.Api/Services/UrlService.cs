using zapURL.Api.Data.Repositories.ShortenUrlRepository;
using zapURL.Api.Models;
using zapURL.Api.Utilities;

namespace zapURL.Api.Services;

internal class UrlService : IUrlService
{
    private readonly IShortUrlRepository _shortUrlRepository;

    public UrlService(IShortUrlRepository shortUrlRepository)
    {
        _shortUrlRepository = shortUrlRepository;
    }

    public async Task<string> GetOriginalUrlAsync(string code)
    {
        return await _shortUrlRepository.GetOriginalUrlAsync(code);
    }

    public async Task<List<ShortUrl>> GetShortUrlsAsync()
    {
        return await _shortUrlRepository.GetAllShortUrlsAsync();
    }

    public async Task<string> ShortenUrlAsync(string originalUrl)
    {
        var code = CodeGenerator.GenerateCode();
        var res = await _shortUrlRepository.AddShortUrlAsync(new ShortUrl
        {
            Id = Guid.CreateVersion7(),
            Code = code,
            OriginalUrl = originalUrl,
            CreatedAt = DateTime.UtcNow
        });

        return res ? code : string.Empty;
    }

    public async Task UpdateUrlAsync(Guid id, string originalUrl)
    {
        await _shortUrlRepository.UpdateUrlAsync(id, originalUrl);
    }

    public async Task Delete(Guid id)
    {
        await _shortUrlRepository.DeleteAsync(id);
    }
}