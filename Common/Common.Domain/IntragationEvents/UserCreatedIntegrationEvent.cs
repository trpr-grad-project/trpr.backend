using System.Dynamic;
using Common.Domain;

namespace Common.Domain.IntragationEvents;

public class SendMessageIntegrationEvent(
    DateTime CreatedOnUtc,
    bool NotifyEmail,
    bool NotifyPhone,
    bool NotifySystem,
    string TemplateType,
    Guid FromUserId,
    ICollection<Guid> ToUserIds,
    IDictionary<string, object> KeyValuePairs)
{
    public Guid FromUserId { get; set; } = FromUserId;
    public ICollection<Guid> ToUserIds { get; set; } = ToUserIds;
    public DateTime CreatedOnUtc { get; set; } = CreatedOnUtc;
    public string TemplateType { get; set; } = TemplateType;
    public bool NotifyEmail { get; set; } = NotifyEmail;
    public bool NotifyPhone { get; set; } = NotifyPhone;
    public bool NotifySystem { get; set; } = NotifySystem;
    public IDictionary<string, object> KeyValuePairs { get; set; } = KeyValuePairs;
}
public class UserCreatedIntegrationEvent(Guid Id, DateTime CreatedOnUtc, Guid UserId, string UserName, string FirstName, string LastName) : IntegrationEvent(Id, CreatedOnUtc)
{
    public Guid UserId { get; set; } = UserId;
    public string UserName { get; set; } = UserName;
    public string FirstName { get; set; } = FirstName;
    public string LastName { get; set; } = LastName;
}
