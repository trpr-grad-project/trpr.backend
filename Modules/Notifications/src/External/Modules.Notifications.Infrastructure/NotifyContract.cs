using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Application.Services;
using Modules.Notifications.Contracts.Contracts;
using Modules.Notifications.Contracts.Dtos;
using Modules.Notifications.Domain.Entities;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Infrastructure;

public class NotifyContract(
    ILogger<NotifyContract> logger,
    RepositoryFactory repositoryFactory,
    IEmailService emailService,
    IUnitOfWork unitOfWork) : INotifiyContract
{
    public async Task NotifyAsync(SystemNotifyRequestDto request, CancellationToken cancellationToken)
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Templates",
            $"{request.TemplateType}.html");

        var (subject, content) = await LoadTemplate(request.TemplateType, cancellationToken);

        ICollection<User> users =
            repositoryFactory.Repository<User>().GetQueryable()
            .Where(x => request.ToUserIds.Contains(x.Id))
            .ToList();

        await NotifyUsers(request, users, content, cancellationToken);
        await NotifyEmails(request, subject, content, cancellationToken);
        NotifyPhones(request, content, cancellationToken);
    }
    public static async Task<(string, string)> LoadTemplate(TemplateType templateType, CancellationToken cancellationToken)
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Templates",
            $"{templateType}.html");

        var text = await File.ReadAllTextAsync(path, cancellationToken);
        var parts = text.Split("---", 3, StringSplitOptions.TrimEntries);

        var metadata = parts[1];
        var body = parts[2];

        var title = metadata
            .Split('\n')
            .First(x => x.StartsWith("title:"))
            .Replace("title:", "")
            .Trim();
        return (title, body);
    }
    private void NotifyPhones(SystemNotifyRequestDto request, string content, CancellationToken cancellationToken)
    {
        var paredTemplate = Scriban.Template.Parse(content);
        var renderedContent = paredTemplate.Render(request.KeyValuePairs);

        foreach (var phone in request.ToPhoneNumbers)
        {
            logger.LogInformation("Sending SMS to {PhoneNumber} with content: {Content}", phone, renderedContent);
        }
    }

    private async Task NotifyEmails(SystemNotifyRequestDto request, string title, string content, CancellationToken cancellationToken)
    {
        var paredTemplate = Scriban.Template.Parse(content);
        var renderedContent = paredTemplate.Render(request.KeyValuePairs);

        foreach (var email in request.ToEmails)
        {
            logger.LogInformation("Sending Email to {Email} with title: {Title}\nContent:\n {Content}", email, title, renderedContent);
            await emailService.SendAsync(
                [email],
                title,
                renderedContent,
                cancellationToken
            );
        }
    }

    private async Task NotifyUsers(SystemNotifyRequestDto request, ICollection<User> users, string content, CancellationToken cancellationToken)
    {
        foreach (var user in users)
        {
            var paredTemplate = Scriban.Template.Parse(content);
            IEnumerable<KeyValuePair<string, string>> newKeyValuePairs = new Dictionary<string, string>(request.KeyValuePairs)
            {
                ["FirstName"] = user.FirstName,
                ["LastName"] = user.LastName,
                ["UserName"] = user.UserName,
                ["Email"] = user.Email ?? string.Empty,
                ["PhoneNumber"] = user.PhoneNumber ?? string.Empty
            };
            newKeyValuePairs = newKeyValuePairs.Union(request.KeyValuePairs);
            var renderedContent = paredTemplate.Render(newKeyValuePairs);

            var notification = Notification.Create(
                renderedContent,
                ContentType.Html,
                request.NotifyEmail,
                request.NotifyPhone,
                request.NotifySystem,
                user.Id
            );
            repositoryFactory.Repository<Notification>().Add(notification);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);

    }

    public async Task UpdateNotificationSettingsAsync(Guid userId, UpdateNotificationSettingsRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(x => x.Id == userId) ?? throw new NotFoundException("User.NotFound", userId);
        user.UpdateNotificationSettings(
            request.TripUpdates,
            request.Messages,
            request.Promotions
        );
        repositoryFactory.Repository<User>().Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<NotificationSettingsResponseDto> GetNotificationSettingsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(x => x.Id == userId) ?? throw new NotFoundException("User.NotFound", userId);
        return new NotificationSettingsResponseDto
        {
            TripUpdates = user.TripUpdates,
            Messages = user.Messages,
            Promotions = user.Promotions
        };
    }
}
