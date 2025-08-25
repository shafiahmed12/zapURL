using ErrorOr;
using zapURL.Api.Contracts.Authentication;

namespace zapURL.Api.Services.AuthenticationService;

public interface IAuthenticationService
{
    Task<ErrorOr<AuthenticationResponse>> SignUpAsync(string email, string password, string callBackUrl);
    Task<ErrorOr<AuthenticationResponse>> SignInAsync(string email, string password);
}
