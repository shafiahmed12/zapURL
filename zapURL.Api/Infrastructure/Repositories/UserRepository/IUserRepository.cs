using ErrorOr;
using zapURL.Api.Models;

namespace zapURL.Api.Infrastructure.Repositories.UserRepository;

public interface IUserRepository
{
    Task<ErrorOr<User>> GetUserById(Guid id);
    Task<ErrorOr<User>> GetUserByEmail(string email);
    Task<ErrorOr<Guid>> AddUser(User user);
}
