using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using zapURL.Api.Data;
using zapURL.Api.Data.Repositories.ShortenUrlRepository;
using zapURL.Api.Services;
using zapURL.Api.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ZapUrlDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddScoped<IShortUrlRepository, ShortUrlRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await DatabaseMigrator.ApplyMigrations<ZapUrlDbContext>(scope);
}

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Theme = ScalarTheme.Mars;
    options.Title = "ZapUrl API Reference";
});


app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapControllers();

await app.RunAsync();