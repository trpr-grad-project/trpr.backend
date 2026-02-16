using Modules.Notifications.Application.Dtos.Responses;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Application.Mappers;

public static class TemplatePaginationMapper
{
    public static TemplatePaginationResponseDto ToPaginationResponseDto(this Template template)
    {
        if (template == null) return null!;

        return new TemplatePaginationResponseDto()
        {
            Id = template.Id,
            UserId = template.UserId,
            Active = template.Active,
            ContentType = template.ContentType
        };
    }
}