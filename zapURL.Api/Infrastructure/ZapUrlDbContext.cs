using Microsoft.EntityFrameworkCore;
using zapURL.Api.Models;

namespace zapURL.Api.Infrastructure;

public class ZapUrlDbContext : DbContext
{
    public ZapUrlDbContext(DbContextOptions<ZapUrlDbContext> options) : base(options)
    {
    }

    public DbSet<ShortUrl> ShortUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortUrl>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Code).IsUnique();
        });
    }
}