using ErrorOr;

namespace zapURL.Api.Errors;

public static class ShortUrlErrors
{
    public static readonly Error UrlNotFoundError = Error.NotFound(
        "Url.NotFound",
        "Url not found");

    public static readonly Error CodeExists = Error.Conflict(
        "Url.CodeExists",
        "Short Url code already exists.");

    public static readonly Error InvalidUrl = Error.Validation(
        "Url.Invalid",
        "Invalid Url");

    public static readonly Error InvalidId = Error.Validation(
        "Url.InvalidId",
        "Invalid Id");

    public static readonly Error EmptyCode = Error.Validation(
        "Url.CodeEmpty",
        "Code cannot be empty");
}