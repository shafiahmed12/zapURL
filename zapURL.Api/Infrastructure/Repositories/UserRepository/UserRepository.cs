using ErrorOr;
using Microsoft.EntityFrameworkCore;
using zapURL.Api.Errors;
using zapURL.Api.Models;

namespace zapURL.Api.Infrastructure.Repositories.UserRepository;

public class UserRepository(ZapUrlDbContext dbContext) : IUserRepository
{
    private readonly ZapUrlDbContext _dbContext = dbContext;

    public async Task<ErrorOr<Guid>> AddUser(User user)
    {
        var userExists = await _dbContext.Users.AsNoTracking().AnyAsync(x => x.Email == user.Email);
        if (userExists) return UserErrors.UserExistsError;

        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user.Id;
    }

    public async Task<ErrorOr<User>> GetUserByEmail(string email)
    {
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
        if (user is null) return UserErrors.UserNotFoundError;

        return user;
    }

    public async Task<ErrorOr<User>> GetUserById(Guid id)
    {
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (user is null) return UserErrors.UserNotFoundError;

        return user;
    }

}
