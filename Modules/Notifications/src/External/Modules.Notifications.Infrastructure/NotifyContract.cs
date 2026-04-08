using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Contracts.Contracts;
using Modules.Notifications.Contracts.Dtos;
using Modules.Notifications.Domain.Entities;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Infrastructure;

public class NotifyContract(
    RepositoryFactory repositoryFactory,
    IUnitOfWork unitOfWork) : INotifiyContract
{
    public async Task NotifyAsync(SystemNotifyRequestDto request, CancellationToken cancellationToken)
    {
        var templateType = Enum.TryParse(typeof(TemplateType), request.TemplateType, out var result) ? (TemplateType)result : throw new ArgumentException("Invalid template type");
        var template = await repositoryFactory.Repository<Template>().GetQueryable()
            .Include(x => x.TemplateLangs)
            .Where(x => x.TemplateType == templateType && x.Active == true)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new NotFoundException("Template.NotFound", templateType.ToString());
        var templateLange = template
            .TemplateLangs
            .FirstOrDefault(x => x.LangCode == request.LangCode) ??
            template
            .TemplateLangs
            .FirstOrDefault(x => x.LangCode == "en") ??
            throw new NotFoundException("Template.Lang.NotFound", templateType, request.LangCode);
        foreach (var userId in request.ToUserIds)
        {
            var notification = Notification.Create(
                templateLange.Content,
                template.ContentType,
                request.NotifyEmail,
                request.NotifyPhone,
                request.NotifySystem,
                userId
            );
            repositoryFactory.Repository<Notification>().Add(notification);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

}
