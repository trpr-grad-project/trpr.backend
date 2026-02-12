using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Application.Dtos.Requests
{
    public class UpdateTemplateDto
    {
        public string? Content { get; set; } = null;
        public TemplateType? TemplateType { get; set; }
        public ContentType? ContentType { get; set; }
    }
}