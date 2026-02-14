using Modules.Notifications.Application.Dtos.Responses;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Application.Mappers;

public static class TemplateMapper
{
    public static TemplateResponseDto ToResponseDto(this Template template)
    {
        if (template == null) return null!;

        return new TemplateResponseDto()
        {
            Id = template.Id,
            UserId = template.UserId,
            // Content = template.Content,
            Active = template.Active,
            // TemplateType = template.TemplateType,
            ContentType = template.ContentType,
            User = template.User?.ToResponseDto() ?? new UserResponseDto { Id = template.UserId }
        };
    }
}
