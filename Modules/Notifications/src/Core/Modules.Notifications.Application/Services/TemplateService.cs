using System.Linq;
using System.Threading;
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
    public class TemplateService(INotificationsDbContext notificationDbContext, IUnitOfWork unitOfWork)
    {
        public async Task<TemplateResponseDto> CreateTemplate(Guid userId, CreateTemplateDto templateDto, CancellationToken cancellationToken = default)
        {
            User user = await notificationDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken) 
                ?? throw new NotFoundException("User.NotFound", userId);

            var templateLanguageDict = templateDto.Translations.ToDictionary(m => m.LangCode, x => x);
            var titleLanguageDict = templateLanguageDict.ToDictionary(m => m.Key, x => x.Value.Title);
            var contentLanguageDict = templateLanguageDict.ToDictionary(m => m.Key, x => x.Value.Content);

            Template template = Template.Create(templateDto.ContentType, templateDto.TemplateType, user, contentLanguageDict, titleLanguageDict);
            notificationDbContext.Templates.Add(template);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return TemplateMapper.ToResponseDto(template);
        }
        public async Task<TemplateResponseDto> UpdateTemplate(Guid templateId, UpdateTemplateDto templateDto, CancellationToken cancellationToken = default)
        {
            Template template = await notificationDbContext.Templates
                .FirstOrDefaultAsync(t => t.Id == templateId, cancellationToken) 
                ?? throw new NotFoundException("Template.NotFound", templateId);

            if (templateDto.Active == true && !template.Active)
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
                // Replace this part with a helper method 
                var templateLanguageDict = templateDto.Translations.ToDictionary(m => m.LangCode, x => x);
                titleLanguageDict = templateLanguageDict.ToDictionary(m => m.Key, x => x.Value.Title);
                contentLanguageDict = templateLanguageDict.ToDictionary(m => m.Key, x => x.Value.Content);
            }

            template.Update(templateDto.ContentType, templateDto.TemplateType, contentLanguageDict, titleLanguageDict, templateDto.Active);
            notificationDbContext.Templates.Update(template);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return TemplateMapper.ToResponseDto(template);
        }

        public async Task<PaginationDto<TemplatePaginationResponseDto>> TemplatesPagination(PaginateRequestDto dto, CancellationToken cancellationToken = default)
        {
            var query = notificationDbContext.Templates.AsQueryable();
            if (dto.sortBy.HasValue)
            {
                query = dto.sortBy.Value switch
                {
                    SortBy.CreatedAt => query.OrderBy(t => t.CreatedAtUTC),
                    SortBy.UpdatedAt => query.OrderBy(t => t.UpdatedAtUTC),
                    _ => query
                };
            }
            else
            {
                query = query.OrderByDescending(t => t.UpdatedAtUTC);
            }
            if(dto.IsActive == true)
            {
                query = query.Where(t => t.Active);
            }
            if (dto.TemplateType.HasValue)
            {
                query = query.Where(t => t.TemplateType == dto.TemplateType);
            }
            if(!string.IsNullOrEmpty(dto.Search))
            {
                query = query.Where(t => t.TemplateLangs.Any(tl => tl.Title.StartsWith(dto.Search)));
            }
            var totalItems = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(x => x.User)
                .Select(x => x.ToPaginationResponseDto())
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(cancellationToken);

            return PaginationDto<TemplatePaginationResponseDto>.Create(dto.Page, dto.PageSize, totalItems, items);
        }

        public async Task<TemplateResponseDto> TemplateDetails(Guid templateId, Guid userId, CancellationToken cancellationToken)
        {
            Template template = await notificationDbContext.Templates
                .Include(t => t.TemplateLangs)
                .Include(x => x.User)
                .FirstOrDefaultAsync(t => t.UserId == userId && t.Id == templateId, cancellationToken)
                ?? throw new NotFoundException("Template not found");
            return TemplateMapper.ToResponseDto(template);
        }
    }
}