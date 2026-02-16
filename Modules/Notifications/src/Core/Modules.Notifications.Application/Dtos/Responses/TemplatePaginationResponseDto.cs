using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Application.Dtos.Responses;

public class TemplatePaginationResponseDto 
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public bool Active { get; set; }
    public ContentType ContentType { get; set; }
    public UserResponseDto User { get; set; } = default!;
}
