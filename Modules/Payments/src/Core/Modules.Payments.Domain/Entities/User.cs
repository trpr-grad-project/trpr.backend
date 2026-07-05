using System.Net.Mail;
using Modules.Payments.Domain.Abstractions;

namespace Modules.Payments.Domain.Entities;

public class User : Entity
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public double Balance { get; private set; } = 0;
    public static User Create(Guid Id, string UserName, string FirstName, string LastName)
    {
        return new User
        {
            Id = Id,
            UserName = UserName,
            FirstName = FirstName,
            LastName = LastName,
            Email = MailAddress.TryCreate(UserName, out var _) ? UserName : null,
            PhoneNumber = MailAddress.TryCreate(UserName, out var _) ? null : UserName,
        };
    }

    public void Charge(double ammount)
    {
        Balance -= ammount;
    }

    public void Recive(double ammount)
    {
        Balance += ammount;
    }
}
