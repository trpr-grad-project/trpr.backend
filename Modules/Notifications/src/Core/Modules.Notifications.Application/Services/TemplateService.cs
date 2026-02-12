using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Application.Dtos;
using Modules.Notifications.Application.Dtos.Requests;
using Modules.Notifications.Application.Exceptions;
using Modules.Notifications.Domain.Entities;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Application.Services
{
    public class TemplateService(INotificationDbContext notificationDbContext, IUnitOfWork unitOfWork)
    {
        public async Task<Guid> CreateTemplate(Guid userId,CreateTemplateDto templateDto, CancellationToken cancellationToken = default)
        { 
            User user = await notificationDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken) ?? throw new NotFoundException("User.NotFound", userId);
            string content = templateDto.Content;
            TemplateType templateType = templateDto.TemplateType;
            ContentType contentType = templateDto.ContentType;
            Template template = Template.Create(content, templateType, contentType, user);
            notificationDbContext.Templates.Add(template);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return template.Id;
        }


        public async Task<Guid> UpdateTemplate(Guid templateId, UpdateTemplateDto templateDto, CancellationToken cancellationToken = default)
        {
            Template template = await notificationDbContext.Templates.FirstOrDefaultAsync(t => t.Id == templateId, cancellationToken) ?? throw new NotFoundException("Template.NotFound", templateId);
            template.Update(templateDto.Content, templateDto.TemplateType, templateDto.ContentType);
            notificationDbContext.Templates.Update(template);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return template.Id;
        }
    }
}