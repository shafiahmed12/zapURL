using System.Text.Json.Serialization;

namespace zapURL.Api.Errors;

public record StackAuthError(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("error")] string Error);