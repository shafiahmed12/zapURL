using System.ComponentModel.DataAnnotations;

namespace zapURL.Api.Contracts.ShortUrl;

public record ShortenUrlRequest(
    [Required] [Url] string Url);