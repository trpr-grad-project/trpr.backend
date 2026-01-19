using Modules.Users.Domain.Events;
using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Entities;

public class User : Entity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdentityProviderId { get; set; } = string.Empty;
    public bool TwoFactorEnabled { get; set; } = false;
    public bool IsVerified { get; set; } = false;
    public virtual Profile Profile { get; set; } = default!;
    public static User Create(string email, string firstName, string lastName, string identityProviderId)
    {
        var user = new User
        {
            Id = Guid.Parse(identityProviderId),
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            IdentityProviderId = identityProviderId,
            TwoFactorEnabled = false,
        };
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        return user;
    }
}
