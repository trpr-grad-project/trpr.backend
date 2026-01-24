using Modules.Users.Domain.Events;
using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Entities;

public class User : Entity
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdentityProviderId { get; set; } = string.Empty;
    public bool TwoFactorEnabled { get; set; } = false;
    public bool IsVerified { get; set; } = false;
    public virtual Profile Profile { get; set; } = default!;
    public virtual ICollection<Token> Tokens { get; set; } = [];
    public static User Create(string UserName, string firstName, string lastName, string identityProviderId)
    {
        var user = new User
        {
            Id = Guid.Parse(identityProviderId),
            UserName = UserName,
            FirstName = firstName,
            LastName = lastName,
            IdentityProviderId = identityProviderId,
            TwoFactorEnabled = false,
            IsVerified = false,
        };
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        return user;
    }
}
