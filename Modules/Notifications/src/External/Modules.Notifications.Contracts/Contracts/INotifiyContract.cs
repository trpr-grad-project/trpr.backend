using Modules.Notifications.Contracts.Dtos;

namespace Modules.Notifications.Contracts.Contracts;

public interface INotifiyContract
{
    public Task NotifyAsync(SystemNotifyRequestDto request, CancellationToken cancellationToken = default);
}
