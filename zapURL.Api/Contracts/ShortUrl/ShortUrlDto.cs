namespace zapURL.Api.Contracts.ShortUrl;

public record ShortUrlDto(Guid Id, string ShortUrl, string OriginalUrl);