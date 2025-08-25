using System.ComponentModel.DataAnnotations;

namespace zapURL.Api.Contracts.Authentication;

public record class RegisterRequest(
    [Required][EmailAddress] string Email,
    [Required] string Password);
