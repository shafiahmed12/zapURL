using System.Text.Json;
using ErrorOr;
using zapURL.Api.Contracts.Authentication;
using zapURL.Api.Errors;
using zapURL.Api.Infrastructure.AuthClient;

namespace zapURL.Api.Services.AuthenticationService;

public class AuthenticationService(
    IStackAuthClient stackAuthClient) : IAuthenticationService
{
    private readonly IStackAuthClient _stackAuthClient = stackAuthClient;

    public async Task<ErrorOr<AuthenticationResponse>> SignInAsync(string email, string password)
    {
        var response = await _stackAuthClient.SignInAsync(email, password);
        if (response.IsError) return response.Errors;

        var result = JsonSerializer.Deserialize<AuthenticationResponse>(response.Value);
        if (result is null)
            return AuthenticationErrors.DeserializationError;

        return new AuthenticationResponse(result.AccessToken, result.RefreshToken, result.StackAuthUserId);
    }

    public async Task<ErrorOr<AuthenticationResponse>> SignUpAsync(string email, string password, string callBackUrl)
    {
        var response = await _stackAuthClient.SignUpAsync(email, password, callBackUrl);
        if (response.IsError) return response.Errors;

        var result = JsonSerializer.Deserialize<AuthenticationResponse>(response.Value);
        if (result is null)
            return AuthenticationErrors.DeserializationError;

        return new AuthenticationResponse(result.AccessToken, result.RefreshToken, result.StackAuthUserId);
    }
}