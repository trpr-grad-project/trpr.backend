using Microsoft.EntityFrameworkCore;
using Modules.Users.Domain.Entities;
using Modules.Users.Application.Repositories;
using Modules.Users.Persistence.Data;

namespace Modules.Users.Persistence.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public Task<User?> GetByEmail(string email)
        {
            return context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}