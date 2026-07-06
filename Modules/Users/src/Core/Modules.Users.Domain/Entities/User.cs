using Modules.Users.Domain.Events;
using Modules.Users.Domain.Abstractions;
using Modules.Users.Domain.Enums;

namespace Modules.Users.Domain.Entities;

public class User : Entity
{
    public Guid Id { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool TwoFactorEnabled { get; private set; } = false;
    public bool IsVerified { get; private set; } = false;
    public virtual Profile Profile { get; set; } = default!;
    public virtual ICollection<Token> Tokens { get; set; } = [];
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    public static User Create(string userName, string firstName, string lastName, string passwordHash)
    {
        var user = new User
        {
            Id = Guid.CreateVersion7(),
            UserName = userName,
            FirstName = firstName,
            LastName = lastName,
            PasswordHash = passwordHash,
            TwoFactorEnabled = false,
            IsVerified = false,
        };
        return user;
    }

    public void Update(string? firstName, string? lastName)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
            this.FirstName = firstName;

        if (!string.IsNullOrWhiteSpace(lastName))
            this.LastName = lastName;
    }

    public void Verify()
    {
        this.IsVerified = true;
        this.RaiseDomainEvent(new UserCreatedDomainEvent(Id));
    }

    public void SetPasswordHash(string passwordHash)
    {
        this.PasswordHash = passwordHash;
    }
}
