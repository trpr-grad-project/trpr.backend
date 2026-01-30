using Microsoft.EntityFrameworkCore;
using Modules.Users.Domain.Entities;
using Modules.Users.Application.Repositories;
using Modules.Users.Infrastructure.Data;

namespace Modules.Users.Infrastructure.Repositories
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