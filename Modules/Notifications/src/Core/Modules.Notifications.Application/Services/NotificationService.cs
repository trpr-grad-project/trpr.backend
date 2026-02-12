using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Application.Dtos;
using Modules.Notifications.Application.Dtos.Requests;
using Modules.Notifications.Domain.Entities;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Application.Services
{
    public class NotificationService(INotificationDbContext notificationDbContext, ILogger<NotificationService> logger, IUnitOfWork unitOfWork)
    {
        private readonly INotificationDbContext notificationDbContext = notificationDbContext;
        private readonly ILogger<NotificationService> logger = logger;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Guid> CreateTemplate(CreateTemplateDto templateDto, CancellationToken cancellationToken)
        {
            var existingTemplate = await notificationDbContext.Templates.FirstOrDefaultAsync(t => t.UserId == templateDto.Id);
            if (existingTemplate != null)
            {
                throw new Exception("User has an existing template");
            }
            User? user = await notificationDbContext.Users.FirstOrDefaultAsync(u => u.Id == templateDto.Id, cancellationToken);
            string content = templateDto.Content;
            TemplateType templateType = templateDto.TemplateType;
            ContentType contentType = templateDto.ContentType;
            Template template = Template.Create(content, templateType, contentType, user!);
            notificationDbContext.Templates.Add(template);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return templateDto.Id;
        }
    }
}