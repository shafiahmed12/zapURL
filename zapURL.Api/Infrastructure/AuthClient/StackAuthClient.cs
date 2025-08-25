using System.Text.Json;
using ErrorOr;
using Serilog;
using zapURL.Api.Errors;

namespace zapURL.Api.Infrastructure.AuthClient;

public class StackAuthClient(HttpClient httpClient) : IStackAuthClient
{
    private readonly HttpClient _httpClient = httpClient;

    public HttpClient GetStackAuthClient()
    {
        return _httpClient;
    }

    public async Task<ErrorOr<string>> SignInAsync(string email, string password)
    {
        try
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(StackAuthRoutes.SignIn, new { email, password });
            var result = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
                return result;

            Log.Error("Error occurred during signIn from Stack Auth : {Result}", result);
            var stackAuthError = JsonSerializer.Deserialize<StackAuthError>(result)!;
            return AuthenticationErrors.AuthenticationError(stackAuthError);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred during signIn from Stack Auth", ex.Message);
            return Error.Unexpected(ex.Message);
        }
    }

    public async Task<ErrorOr<string>> SignUpAsync(string email, string password, string callBackUrl)
    {
        try
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(StackAuthRoutes.SignUp,
                new { email, password, verification_callback_url = callBackUrl });
            var result = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
                return result;

            Log.Error("Error occurred during signIn from Stack Auth : {Result}", result);
            var stackAuthError = JsonSerializer.Deserialize<StackAuthError>(result)!;
            return AuthenticationErrors.AuthenticationError(stackAuthError);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred during signIn from Stack Auth", ex.Message);
            return Error.Unexpected(ex.Message);
        }
    }
}