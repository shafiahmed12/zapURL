using Microsoft.AspNetCore.Mvc;
using Serilog;
using zapURL.Api.Contracts.ShortUrl;
using zapURL.Api.Errors;
using zapURL.Api.Services.UrlService;

namespace zapURL.Api.Controllers;

[Route("urls")]
public class UrlController(IUrlService urlService) : BaseController
{
    private readonly IUrlService _urlService = urlService;

    [HttpPost("shorten")]
    public async Task<IActionResult> ShortenUrl(ShortenUrlRequest request)
    {
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var url))
        {
            Log.Error("Provided url is invalid {Url}", request.Url);
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
            Log.Error("Code cannot be empty");
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
            Log.Error("Invalid Id: {Id}", id);
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
            Log.Error("Invalid Id: {Id}", id);
            return Problem([ShortUrlErrors.InvalidId]);
        }

        var response = await _urlService.Delete(id);
        return response.Match(
            _ => NoContent(),
            Problem
        );
    }
}