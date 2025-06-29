using Microsoft.AspNetCore.Mvc;
using zapURL.Api.Dtos.Requests;
using zapURL.Api.Services;

namespace zapURL.Api.Controllers;

[ApiController]
[Route("")]
public class UrlController : ControllerBase
{
    private readonly IShortenUrlService _shortenUrlService;

    public UrlController(IShortenUrlService shortenUrlService)
    {
        _shortenUrlService = shortenUrlService;
    }

    [HttpPost("shorten")]
    public async Task<IActionResult> ShortenUrl(ShortenUrlRequest request)
    {
        var shortenedCode = await _shortenUrlService.ShortenUrlAsync(request.LongUrl);
        return CreatedAtAction(nameof(RedirectUrl), new { code = shortenedCode }, shortenedCode);
    }

    [HttpGet("{code}")]
    public IActionResult RedirectUrl(string code)
    {
        var originalUrl = _shortenUrlService.GetByCodeAsync(code);
        return Redirect(originalUrl);
    }
}