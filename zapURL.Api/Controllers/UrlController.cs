using Microsoft.AspNetCore.Mvc;
using zapURL.Api.Dtos;
using zapURL.Api.Dtos.Requests;
using zapURL.Api.Services;

namespace zapURL.Api.Controllers;

[ApiController]
[Route("")]
public class UrlController : ControllerBase
{
    private readonly ILogger<UrlController> _logger;
    private readonly IUrlService _urlService;

    public UrlController(IUrlService urlService, ILogger<UrlController> logger)
    {
        _urlService = urlService;
        _logger = logger;
    }

    [HttpPost("shorten")]
    public async Task<IActionResult> ShortenUrl(ShortenUrlRequest request)
    {
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var result))
        {
            _logger.LogError("provided url is invalid {0}", request.Url);
            return BadRequest();
        }

        var code = await _urlService.ShortenUrlAsync(result!.ToString());
        return CreatedAtAction(nameof(RedirectUrl), new { code }, code);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> RedirectUrl(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            _logger.LogError("Code cannot be empty");
            return BadRequest();
        }

        var originalUrl = await _urlService.GetOriginalUrlAsync(code);
        if (string.IsNullOrWhiteSpace(originalUrl))
        {
            _logger.LogError("Original URL not found for code {0}", code);
            return NotFound();
        }

        return Redirect(originalUrl);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var res = await _urlService.GetShortUrlsAsync();
        var shortUrls = res.ConvertAll(x => new ShortUrlDto(
            x.Id,
            Url.Action(nameof(RedirectUrl), "Url", new { code = x.Code }, Request.Scheme) ?? string.Empty,
            x.OriginalUrl
        ));

        return Ok(shortUrls);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUrl(Guid id, UpdateUrlRequest updateUrlRequest)
    {
        if (id == Guid.Empty || !Guid.TryParse(id.ToString(), out var result))
            return BadRequest(
                $"Invalid Id: {id}");

        await _urlService.UpdateUrlAsync(result, updateUrlRequest.OriginalUrl);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty || !Guid.TryParse(id.ToString(), out var result))
            return BadRequest(
                $"Invalid Id: {id}");

        await _urlService.Delete(result);
        return NoContent();
    }
}