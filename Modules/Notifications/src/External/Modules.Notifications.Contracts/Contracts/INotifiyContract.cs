using Modules.Notifications.Contracts.Dtos;

namespace Modules.Notifications.Contracts.Contracts;

public interface INotifiyContract
{
    public Task NotifyAsync(SystemNotifyRequestDto request, CancellationToken cancellationToken = default);
    public Task UpdateNotificationSettingsAsync(Guid userId, UpdateNotificationSettingsRequestDto request, CancellationToken cancellationToken = default);
}

public class UpdateNotificationSettingsRequestDto
{
    public bool? TripUpdates { get; set; } = true;
    public bool? Messages { get; set; } = true;
    public bool? Promotions { get; set; } = true;
}