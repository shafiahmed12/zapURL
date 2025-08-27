using ErrorOr;

namespace zapURL.Api.Errors;

public static class UserErrors
{
    public static readonly Error UserNotFoundError = Error.NotFound(
        "User.NotFound",
        "No user found"
    );

    public static readonly Error UserExistsError = Error.Conflict(
        "User.Exists",
        "User with the email already exists"
    );
}
