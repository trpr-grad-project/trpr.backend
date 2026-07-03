using Common.Application.Dtos;
using Microsoft.EntityFrameworkCore;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Application.Dtos.Requests;
using Modules.Notifications.Application.Dtos.Responses;
using Modules.Notifications.Application.Mappers;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Application.Services
{
    public class NotificationService(RepositoryFactory repositoryFactory)
    {
        public async Task<CursorPageDto<NotificationResponseDto, Guid?>> GetNotificationsAsync(Guid userId, QueryNotificationsRequestDto request)
        {
            var ret = repositoryFactory
                    .Repository<Notification>()
                    .GetQueryable()
                    .Where(x => x.UserId == userId);

            if (request.LastNotificationId.HasValue)
                ret = ret
                    .Where(x => x.Id < request.LastNotificationId.Value);

            var res = await ret
                    .OrderByDescending(x => x.Id)
                    .Take(request.PageSize ?? 20)
                    .Select(x => x.ToResponseDto())
                    .ToListAsync();

            return new CursorPageDto<NotificationResponseDto, Guid?>
            {
                Items = res,
                NextCursor = res.LastOrDefault()?.Id
            };
        }
    }
}