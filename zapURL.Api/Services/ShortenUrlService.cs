using zapURL.Api.Data;
using zapURL.Api.Models;
using zapURL.Api.Utilities;

namespace zapURL.Api.Services;

internal sealed class ShortenUrlService : IShortenUrlService
{
    private readonly ZapUrlDbContext _dbContext;

    public ShortenUrlService(ZapUrlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> ShortenUrlAsync(string url)
    {
        var code = CodeGenerator.GenerateCode();
        var shortUrl = new ShortUrl
        {
            Id = Guid.CreateVersion7(),
            OriginalUrl = url,
            CreatedAt = DateTime.UtcNow,
            Code = code
        };

        _dbContext.ShortUrls.Add(shortUrl);

        await _dbContext.SaveChangesAsync();

        return code;
    }

    public string GetByCodeAsync(string code)
    {
        return _dbContext.ShortUrls.Where(x => x.Code == code).Select(x => x.OriginalUrl).First();
    }
}