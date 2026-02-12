using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Domain.Entities;

public class Template
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool Active { get; set; }
    public TemplateType TemplateType { get; set; }
    public ContentType ContentType { get; set; } = ContentType.Pure;
    public User User { get; set; } = default!;
    public static Template Create(string content, TemplateType templateType, ContentType contentType, User user)
    {
        return new Template
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Content = content,
            TemplateType = templateType,
            ContentType = contentType,
            User = user,
        };
    }
}
