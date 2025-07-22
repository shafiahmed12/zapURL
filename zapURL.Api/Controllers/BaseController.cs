using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace zapURL.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        HttpContext.Items["errors"] = errors;
        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };

        var problemDetails = new ProblemDetails
        {
            Title = firstError.Description,
            Status = statusCode,
            Instance = HttpContext.Request.Path,
            Type = nameof(firstError.Type)
        };

        var errorList = errors.GroupBy(x => x.Code)
            .ToDictionary(
                g => g.Key.ToLowerInvariant(),
                g => g.Select(x => x.Description).ToArray()
            );

        problemDetails.Extensions.Add("ErrorCodes", errorList);
        problemDetails.Extensions["requestId"] = HttpContext.TraceIdentifier;

        return Problem(title: problemDetails.Title, statusCode: problemDetails.Status,
            instance: problemDetails.Instance, extensions: problemDetails.Extensions);
    }
}