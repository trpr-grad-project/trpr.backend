using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Application.Dtos.Responses;

public class TemplateResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool Active { get; set; }
    public TemplateType TemplateType { get; set; }
    public ContentType ContentType { get; set; }
    public UserResponseDto User { get; set; } = default!;
}
