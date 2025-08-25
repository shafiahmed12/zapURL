using System.Text.Json;
using ErrorOr;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;
using zapURL.Api.Infrastructure.Repositories.ShortenUrlRepository;
using zapURL.Api.Models;
using zapURL.Api.Utilities;

namespace zapURL.Api.Services.UrlService;

internal class UrlService : IUrlService
{
    private readonly IDistributedCache _cache;
    private readonly IShortUrlRepository _shortUrlRepository;

    public UrlService(IShortUrlRepository shortUrlRepository, IDistributedCache cache)
    {
        _shortUrlRepository = shortUrlRepository;
        _cache = cache;
    }

    public async Task<ErrorOr<string>> GetOriginalUrlAsync(string code)
    {
        var cachedResult = await _cache.GetStringAsync(code);

        if (cachedResult is not null) return cachedResult;

        var result = await _shortUrlRepository.GetOriginalUrlAsync(code);

        if (result.IsError) return result.Errors;

        await _cache.SetStringAsync(code, result.Value);

        return result.Value;
    }

    public async Task<ErrorOr<List<ShortUrl>>> GetShortUrlsAsync()
    {
        var cachedResult = await _cache.GetStringAsync("All");

        if (cachedResult is not null)
        {
            var urls = JsonSerializer.Deserialize<List<ShortUrl>>(cachedResult);
            if (urls is not null) return urls;

            Log.Error("Failed to Failed to deserialize cached short URLs {Urls}.", new { cachedResult });
            return Error.Failure("DeserializationError", "Failed to deserialize cached short URLs.");
        }

        var result = await _shortUrlRepository.GetAllShortUrlsAsync();

        if (result.IsError) return result.Errors;

        await _cache.SetStringAsync("All", JsonSerializer.Serialize(result.Value),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20) });

        return result.Value;
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
        Log.Information("created short url with code: {Code} and originalURl: {OriginalUrl}", result.Value,
            originalUrl);
        await _cache.RemoveAsync("All");
        return result.Value;
    }

    public async Task<ErrorOr<string>> UpdateUrlAsync(Guid id, string originalUrl)
    {
        var result = await _shortUrlRepository.UpdateUrlAsync(id, originalUrl);
        if (result.IsError) return result.Errors;
        Log.Information("updated short url with Id: {Id} code: {Code} and originalURl: {Original}", id, result.Value,
            originalUrl);
        await _cache.RemoveAsync("All");
        return result.Value;
    }

    public async Task<ErrorOr<string>> Delete(Guid id)
    {
        var result = await _shortUrlRepository.DeleteAsync(id);
        if (result.IsError) return result.Errors;
        Log.Information("Deleted short url with Id: {Id} and code: {Code}", id, result.Value);
        await _cache.RemoveAsync("All");
        await _cache.RemoveAsync(result.Value);
        return result.Value;
    }
}