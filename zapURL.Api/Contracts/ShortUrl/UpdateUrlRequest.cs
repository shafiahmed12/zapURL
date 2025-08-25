using System.ComponentModel.DataAnnotations;

namespace zapURL.Api.Contracts.ShortUrl;

public record UpdateUrlRequest(
    [Required] [Url] string OriginalUrl);