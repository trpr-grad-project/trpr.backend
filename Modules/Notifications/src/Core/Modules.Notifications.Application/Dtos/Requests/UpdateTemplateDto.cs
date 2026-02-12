using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Application.Services
{
    public class UpdateTemplateDto
    {
        public string? Content { get; set; } = null;
        public TemplateType? TemplateType { get; set; }
        public ContentType? ContentType { get; set; }
    }
}