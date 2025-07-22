using Microsoft.AspNetCore.Mvc;
using zapURL.Api.Dtos;
using zapURL.Api.Dtos.Requests;
using zapURL.Api.Errors;
using zapURL.Api.Services.UrlService;

namespace zapURL.Api.Controllers;

[Route("urls")]
public class UrlController : BaseController
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
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var url))
        {
            _logger.LogError("Provided url is invalid {Url}", request.Url);
            return Problem([ShortUrlErrors.InvalidUrl]);
        }

        var response = await _urlService.ShortenUrlAsync(url.ToString());
        return response.Match(
            result => CreatedAtAction(nameof(RedirectUrl), new { code = result }, result),
            Problem);
    }

    [HttpGet("/{code}")]
    public async Task<IActionResult> RedirectUrl(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            _logger.LogError("Code cannot be empty");
            return Problem([ShortUrlErrors.EmptyCode]);
        }

        var response = await _urlService.GetOriginalUrlAsync(code);
        return response.Match(
            Redirect,
            Problem
        );
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _urlService.GetShortUrlsAsync();
        return response.Match(
            result => Ok(result.ConvertAll(x => new ShortUrlDto(
                x.Id,
                Url.Action(nameof(RedirectUrl), "Url", new { code = x.Code }, Request.Scheme) ?? string.Empty,
                x.OriginalUrl
            ))),
            Problem
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUrl(Guid id, UpdateUrlRequest updateUrlRequest)
    {
        if (id == Guid.Empty)
        {
            _logger.LogError("Invalid Id: {Id}", id);
            return Problem([ShortUrlErrors.InvalidId]);
        }

        var response = await _urlService.UpdateUrlAsync(id, updateUrlRequest.OriginalUrl);
        return response.Match(
            _ => NoContent(),
            Problem
        );
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogError("Invalid Id: {Id}", id);
            return Problem([ShortUrlErrors.InvalidId]);
        }

        var response = await _urlService.Delete(id);
        return response.Match(
            _ => NoContent(),
            Problem
        );
    }
}