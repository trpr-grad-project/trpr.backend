using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Application.Dtos.Requests
{
    public class CreateTemplateDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public TemplateType TemplateType { get; set; }

        public ContentType ContentType { get; set; }
    }
}
