using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;
using zapURL.Api.Extensions;
using zapURL.Api.Infrastructure;
using zapURL.Api.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilog();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration);

builder.Services.AddControllers();

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
app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseExceptionHandler();
app.MapControllers();
await app.RunAsync();