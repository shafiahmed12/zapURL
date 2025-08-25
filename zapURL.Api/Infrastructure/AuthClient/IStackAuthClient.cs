using ErrorOr;

namespace zapURL.Api.Infrastructure.AuthClient;

public interface IStackAuthClient
{
    HttpClient GetStackAuthClient();
    Task<ErrorOr<string>> SignInAsync(string email, string password);
    Task<ErrorOr<string>> SignUpAsync(string email, string password, string callBackUrl);
}
