using Microsoft.EntityFrameworkCore;
using zapURL.Api.Models;

namespace zapURL.Api.Data;

public class UrlDbContext(DbContextOptions<UrlDbContext> options) : DbContext(options)
{
    public DbSet<ShortUrl> ShortUrls { get; set; } = null!;
}