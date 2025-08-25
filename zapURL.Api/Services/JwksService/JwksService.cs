using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using zapURL.Api.Configurations;

namespace zapURL.Api.Services.JwksService;

public class JwksService(
    IHttpClientFactory httpClientFactory,
    IOptions<StackAuthSettings> stackAuthSettings) : IJwksService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly StackAuthSettings _stackAuthSettings = stackAuthSettings.Value;
    private JsonWebKeySet? _jwks;

    public async Task<JsonWebKeySet> GetJwksAsync()
    {
        if (_jwks is not null)
            return _jwks;

        try
        {
            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync($"{StackAuthSettings.BaseAddress}/api/v1/projects/{_stackAuthSettings.ProjectId}/.well-known/jwks.json");

            var jwks = JsonSerializer.Deserialize<JsonWebKeySet>(response);
            if (jwks is null)
            {
                Log.Error("Deserialized JWKS is null.");
                throw new InvalidOperationException("Deserialized JWKS is null.");
            }
            _jwks = jwks;
            return _jwks;
        }
        catch (JsonException ex)
        {
            Log.Error(ex, "Failed to parse JWKS response: {Message}", ex.Message);
            throw new InvalidOperationException($"Failed to parse JWKS response: {ex.Message}", ex);
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex, "Failed to fetch JWKS from Stack Auth: {Message}", ex.Message);
            throw new InvalidOperationException($"Failed to fetch JWKS from Stack Auth: {ex.Message}", ex);
        }
    }
}
