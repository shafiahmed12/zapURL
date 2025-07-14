using Microsoft.EntityFrameworkCore;
using zapURL.Api.Models;

namespace zapURL.Api.Data.Repositories.ShortenUrlRepository;

internal class ShortUrlRepository : IShortUrlRepository
{
    private readonly ZapUrlDbContext _dbContext;

    public ShortUrlRepository(ZapUrlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GetOriginalUrlAsync(string code)
    {
        var originalUrl = await _dbContext.ShortUrls
            .AsNoTracking()
            .Where(x => x.Code == code)
            .Select(x => x.OriginalUrl)
            .FirstOrDefaultAsync();

        return originalUrl ?? string.Empty;
    }

    public async Task UpdateUrlAsync(Guid id, string originalUrl)
    {
        var shortUrl = await _dbContext.ShortUrls.FirstOrDefaultAsync(x => x.Id == id);
        if (shortUrl is null) throw new ArgumentNullException($"record with Id: {id} not found");
        shortUrl.OriginalUrl = originalUrl;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var shortUrl = await _dbContext.ShortUrls.FirstOrDefaultAsync(x => x.Id == id);
        if (shortUrl is not null)
        {
            _dbContext.ShortUrls.Remove(shortUrl);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> AddShortUrlAsync(ShortUrl shortUrl)
    {
        await _dbContext.ShortUrls.AddAsync(shortUrl);

        var res = await _dbContext.SaveChangesAsync();
        return res == 1;
    }

    public async Task<List<ShortUrl>> GetAllShortUrlsAsync()
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
}