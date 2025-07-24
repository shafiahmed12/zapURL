using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Serilog;
using zapURL.Api.Errors;
using zapURL.Api.Models;

namespace zapURL.Api.Data.Repositories.ShortenUrlRepository;

internal class ShortUrlRepository : IShortUrlRepository
{
    private readonly ZapUrlDbContext _dbContext;

    public ShortUrlRepository(ZapUrlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<string>> GetOriginalUrlAsync(string code)
    {
        var originalUrl = await _dbContext.ShortUrls
            .AsNoTracking()
            .Where(x => x.Code == code)
            .Select(x => x.OriginalUrl)
            .FirstOrDefaultAsync();

        if (originalUrl is not null) return originalUrl;
        Log.Error("OriginalUrl for code: {Code} not found", code);
        return ShortUrlErrors.UrlNotFoundError;
    }

    public async Task<ErrorOr<string>> AddShortUrlAsync(ShortUrl shortUrl)
    {
        // Check for duplicate code
        var exists = await _dbContext.ShortUrls.AnyAsync(x => x.Code == shortUrl.Code);
        if (exists)
        {
            Log.Error("short url code: {Code} already exists", shortUrl.Code);
            return ShortUrlErrors.CodeExists;
        }

        await _dbContext.ShortUrls.AddAsync(shortUrl);
        await _dbContext.SaveChangesAsync();
        return shortUrl.Code;
    }

    public async Task<ErrorOr<List<ShortUrl>>> GetAllShortUrlsAsync()
    {
        return await _dbContext.ShortUrls
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ShortUrl
            {
                Id = x.Id,
                Code = x.Code,
                OriginalUrl = x.OriginalUrl
            })
            .ToListAsync();
    }

    public async Task<ErrorOr<string>> UpdateUrlAsync(Guid id, string originalUrl)
    {
        var shortUrl = await _dbContext.ShortUrls.FirstOrDefaultAsync(x => x.Id == id);
        if (shortUrl is null) return ShortUrlErrors.UrlNotFoundError;
        shortUrl.OriginalUrl = originalUrl;

        await _dbContext.SaveChangesAsync();
        return shortUrl.Code;
    }

    public async Task<ErrorOr<string>> DeleteAsync(Guid id)
    {
        var shortUrl = await _dbContext.ShortUrls.FirstOrDefaultAsync(x => x.Id == id);
        if (shortUrl is null) return ShortUrlErrors.UrlNotFoundError;
        _dbContext.ShortUrls.Remove(shortUrl);

        await _dbContext.SaveChangesAsync();
        return shortUrl.Code;
    }
}