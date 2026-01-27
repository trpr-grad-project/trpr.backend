using Microsoft.EntityFrameworkCore;
using Modules.Users.Domain.Entities;
using Modules.Users.Application.Repositories;
using Modules.Users.Persistence.Data;

namespace Modules.Users.Persistence.Repositories
{
    public class UserRepository(UsersDbContext context) : IUserRepository
    {
        public Task<User?> GetByIdentifier(string identifier)
        {
            return context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.UserName == identifier);
        }
    }
}