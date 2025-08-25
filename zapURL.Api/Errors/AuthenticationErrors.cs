using System.Globalization;
using ErrorOr;

namespace zapURL.Api.Errors;

public static class AuthenticationErrors
{
    public static readonly Error DeserializationError = Error.Failure(
        "Authentication.Deserialization",
        "Could not deserialize the auth response"
    );

    public static Error AuthenticationError(StackAuthError authError)
    {
        var textInfo = CultureInfo.InvariantCulture.TextInfo;

        var code = string.Concat(authError.Code.Split("_").Select(x => textInfo.ToTitleCase(x.ToLower())));
        return Error.Failure(
            $"AuthenticationError.{code}",
            authError.Error
        );
    }
}