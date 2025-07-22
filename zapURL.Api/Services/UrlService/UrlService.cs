using ErrorOr;
using zapURL.Api.Data.Repositories.ShortenUrlRepository;
using zapURL.Api.Models;
using zapURL.Api.Utilities;

namespace zapURL.Api.Services.UrlService;

internal class UrlService : IUrlService
{
    private readonly ILogger<UrlService> _logger;
    private readonly IShortUrlRepository _shortUrlRepository;

    public UrlService(IShortUrlRepository shortUrlRepository, ILogger<UrlService> logger)
    {
        _shortUrlRepository = shortUrlRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<string>> GetOriginalUrlAsync(string code)
    {
        var result = await _shortUrlRepository.GetOriginalUrlAsync(code);
        return result.IsError ? result.Errors : result.Value;
    }

    public async Task<ErrorOr<List<ShortUrl>>> GetShortUrlsAsync()
    {
        var result = await _shortUrlRepository.GetAllShortUrlsAsync();
        return result.IsError ? result.Errors : result.Value;
    }

    public async Task<ErrorOr<string>> ShortenUrlAsync(string originalUrl)
    {
        var result = await _shortUrlRepository.AddShortUrlAsync(new ShortUrl
        {
            Id = Guid.CreateVersion7(),
            Code = CodeGenerator.GenerateCode(),
            OriginalUrl = originalUrl,
            CreatedAt = DateTime.UtcNow
        });
        if (result.IsError) return result.Errors;
        _logger.LogInformation("created short url with code: {Code} and originalURl: {OriginalUrl}", result.Value,
            originalUrl);
        return result.Value;
    }

    public async Task<ErrorOr<string>> UpdateUrlAsync(Guid id, string originalUrl)
    {
        var result = await _shortUrlRepository.UpdateUrlAsync(id, originalUrl);
        if (result.IsError) return result.Errors;
        _logger.LogInformation("updated short url with Id: {Id} code: {Code} and originalURl: {Original}", id,
            result.Value, originalUrl);
        return result.Value;
    }

    public async Task<ErrorOr<string>> Delete(Guid id)
    {
        var result = await _shortUrlRepository.DeleteAsync(id);
        if (result.IsError) return result.Errors;
        _logger.LogInformation("Deleted short url with Id: {Id} and code: {Code}", id, result.Value);
        return result.Value;
    }
}