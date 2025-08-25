using System.Text.Json.Serialization;

namespace zapURL.Api.Contracts.Authentication;

public record AuthenticationResponse(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("refresh_token")] string RefreshToken,
    [property: JsonPropertyName("user_id")] Guid StackAuthUserId);
