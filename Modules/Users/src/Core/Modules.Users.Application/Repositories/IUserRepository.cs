using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Repositories;

public interface IUserRepository
{
    public Task<User?> GetByEmail(string email);
}
