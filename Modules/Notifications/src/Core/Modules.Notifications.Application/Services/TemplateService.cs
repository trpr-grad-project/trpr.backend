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
    public class TemplateService(INotificationDbContext notificationDbContext, IUnitOfWork unitOfWork)
    {
        public async Task<TemplateResponseDto> CreateTemplate(Guid userId, CreateTemplateDto templateDto, CancellationToken cancellationToken = default)
        {
            User user = await notificationDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken) ?? throw new NotFoundException("User.NotFound", userId);
            var templateLanguageDict = templateDto.Translations.ToDictionary(m => m.LangCode, x => x);
            var titleLanguageDict = templateLanguageDict.ToDictionary(m => m.Key, x => x.Value.Title);
            var contentLanguageDict = templateLanguageDict.ToDictionary(m => m.Key, x => x.Value.Content);
            Template template = Template.Create(templateDto.ContentType, user, contentLanguageDict, titleLanguageDict);
            notificationDbContext.Templates.Add(template);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return TemplateMapper.ToResponseDto(template);
        }
        public async Task<TemplateResponseDto> UpdateTemplate(Guid templateId, UpdateTemplateDto templateDto, CancellationToken cancellationToken = default)
        {
            Template template = await notificationDbContext.Templates.FirstOrDefaultAsync(t => t.Id == templateId, cancellationToken) ?? throw new NotFoundException("Template.NotFound", templateId);
            if (templateDto.Active && !template.Active)
            {
                // checks if there are existing and active templates
                var activeTemplate = await notificationDbContext.Templates
                    .FirstOrDefaultAsync(x =>
                        x.UserId == template.UserId &&
                        x.Active &&
                        x.Id != templateId,
                        cancellationToken);
                if (activeTemplate != null)
                {
                    activeTemplate.Active = false;
                }
            }
            Dictionary<string,string>? titleLanguageDict = null;
            Dictionary<string, string>? contentLanguageDict = null;
            if (templateDto.Translations != null) 
            {
                var templateLanguageDict = templateDto.Translations.ToDictionary(m => m.LangCode, x => x);
                titleLanguageDict = templateLanguageDict.ToDictionary(m => m.Key, x => x.Value.Title);
                contentLanguageDict = templateLanguageDict.ToDictionary(m => m.Key, x => x.Value.Content);
            }
            template.Update(templateDto.ContentType, contentLanguageDict, titleLanguageDict, templateDto.Active);
            notificationDbContext.Templates.Update(template);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return TemplateMapper.ToResponseDto(template);
        }

        public async Task<PaginationDto<TemplatePaginationResponseDto>> TemplatesPagination(PaginateRequestDto dto, CancellationToken cancellationToken = default)
        {
            var query = notificationDbContext.Templates.AsQueryable();
            query = query.OrderByDescending(t => t.UpdatedAtUTC);
            var totalItems = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(x => x.User)
                .Select(x => x.ToPaginationResponseDto())
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(cancellationToken);
            return PaginationDto<TemplatePaginationResponseDto>.Create(dto.Page, dto.PageSize, totalItems, items);
        }
    }
}