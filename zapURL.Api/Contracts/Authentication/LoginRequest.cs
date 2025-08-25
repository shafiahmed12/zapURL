using System.ComponentModel.DataAnnotations;

namespace zapURL.Api.Contracts.Authentication;

public record class LoginRequest(
    [Required] [EmailAddress] string Email,
    [Required]string Password);
