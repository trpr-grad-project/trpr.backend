namespace Common.Domain.IntragationEvents;

public class SendSystemMessageIntegrationEvent(
    string Message,
    bool NotifyEmail,
    bool NotifyPhone,
    bool NotifySystem,
    string TemplateType,
    ICollection<Guid> ToUserIds) : IntegrationEvent
{
    public ICollection<Guid> ToUserIds { get; set; } = ToUserIds;
    public string Message { get; set; } = Message;
    public bool NotifyEmail { get; set; } = NotifyEmail;
    public bool NotifyPhone { get; set; } = NotifyPhone;
    public bool NotifySystem { get; set; } = NotifySystem;
    public string TemplateType { get; set; } = TemplateType;
}