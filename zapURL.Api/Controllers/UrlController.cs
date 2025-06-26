using Microsoft.AspNetCore.Mvc;
using zapURL.Api.Dtos.Requests;

namespace zapURL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController : ControllerBase
{
    [HttpPost("shorten")]
    public IActionResult ShortenUrl(ShortenUrlRequest request)
    {
        return Ok(request.LongUrl);
    }

    [HttpGet]
    public IActionResult RedirectUrl(string code)
    {
        return Ok(code);
    }
}