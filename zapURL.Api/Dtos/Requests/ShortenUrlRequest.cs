using System.ComponentModel.DataAnnotations;

namespace zapURL.Api.Dtos.Requests;

public record ShortenUrlRequest([Required] string Url);