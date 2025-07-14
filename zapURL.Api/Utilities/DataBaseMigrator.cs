using Microsoft.EntityFrameworkCore;

namespace zapURL.Api.Utilities;

internal static class DatabaseMigrator
{
    public static async Task ApplyMigrations<TDbContext>(IServiceScope scope) where TDbContext : DbContext
    {
        await using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        await context.Database.MigrateAsync();
    }
}