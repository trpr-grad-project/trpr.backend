using System.Linq;
using Common.Application.Dtos;
using Common.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Application.Dtos;
using Modules.Notifications.Application.Dtos.Requests;
using Modules.Notifications.Application.Dtos.Responses;
using Modules.Notifications.Application.Mappers;
using Modules.Notifications.Domain.Entities;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Application.Services
{
    public class TemplateService(INotificationDbContext notificationDbContext)
    {
        public Task<Guid> CreateTemplate(Guid userId, CreateTemplateDto templateDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
            // User user = await notificationDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken) ?? throw new NotFoundException("User.NotFound", userId);
            // string content = templateDto.Content;
            // TemplateType templateType = templateDto.TemplateType;
            // ContentType contentType = templateDto.ContentType;
            // Template template = Template.Create(content, templateType, contentType, user);
            // notificationDbContext.Templates.Add(template);
            // await unitOfWork.SaveChangesAsync(cancellationToken);
            // return template.Id;
        }
        public Task<Guid> UpdateTemplate(Guid templateId, UpdateTemplateDto templateDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();

            // Template template = await notificationDbContext.Templates.FirstOrDefaultAsync(t => t.Id == templateId, cancellationToken) ?? throw new NotFoundException("Template.NotFound", templateId);
            // template.Update(templateDto.Content, templateDto.TemplateType, templateDto.ContentType);
            // notificationDbContext.Templates.Update(template);
            // await unitOfWork.SaveChangesAsync(cancellationToken);
            // return template.Id;
        }

        public async Task<PaginationDto<TemplateResponseDto>> TemplatesPagination(PaginateRequestDto dto, CancellationToken cancellationToken = default)
        {
            var query = notificationDbContext.Templates.AsQueryable();
            query = query.OrderByDescending(t => t.UpdatedAtUTC);
            var totalItems = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(x => x.User)
                .Select(x => x.ToResponseDto())
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(cancellationToken);
            return PaginationDto<TemplateResponseDto>.Create(dto.Page, dto.PageSize, totalItems, items);
        }
    }
}