using Microsoft.IdentityModel.Tokens;

namespace zapURL.Api.Services.JwksService;

public interface IJwksService
{
    Task<JsonWebKeySet> GetJwksAsync();
}