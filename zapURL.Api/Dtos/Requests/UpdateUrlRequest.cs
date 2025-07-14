using System.ComponentModel.DataAnnotations;

namespace zapURL.Api.Dtos.Requests;

public record UpdateUrlRequest([Required] string OriginalUrl);