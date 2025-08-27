using Microsoft.EntityFrameworkCore;
using zapURL.Api.Models;

namespace zapURL.Api.Infrastructure;

public class ZapUrlDbContext : DbContext
{
    public ZapUrlDbContext(DbContextOptions<ZapUrlDbContext> options) : base(options)
    {
    }

    public DbSet<ShortUrl> ShortUrls { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortUrl>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Code).IsUnique();

            builder.HasOne(s => s.CreatedByUser)
                .WithMany(u => u.ShortUrls)
                .HasForeignKey(s => s.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.StackAuthUserId).IsUnique();

            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.StackAuthUserId).IsRequired();
        });
    }
}