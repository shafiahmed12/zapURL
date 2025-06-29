using Microsoft.EntityFrameworkCore;

namespace zapURL.Api.Data;

public class UrlDbContext : DbContext
{
    public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options)
    {
    }
}