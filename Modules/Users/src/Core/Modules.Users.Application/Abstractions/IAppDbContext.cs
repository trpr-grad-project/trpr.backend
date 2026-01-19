using Microsoft.EntityFrameworkCore;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Abstractions;

public interface IAppDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Interest> Interests { get; set; }
    public DbSet<Vibe> Vibes { get; set; }
    public DbSet<ProfileLanguage> ProfileLanguages { get; set; }
    public DbSet<ProfileInterest> ProfileInterests { get; set; }
    public DbSet<ProfileVibe> ProfileVibes { get; set; }
}
