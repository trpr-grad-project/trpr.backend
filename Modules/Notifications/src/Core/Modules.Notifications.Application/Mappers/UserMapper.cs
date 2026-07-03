using Modules.Notifications.Application.Dtos.Responses;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Application.Mappers;

public static class UserMapper
{
    public static NotificationUserResponseDto ToResponseDto(this User user)
    {
        if (user == null) return null!;

        return new NotificationUserResponseDto
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }
}

public static class NotificationMapper
{
    public static NotificationResponseDto ToResponseDto(this Notification notification)
    {
        if (notification == null) return null!;

        return new NotificationResponseDto
        {
            Id = notification.Id,
            Title = notification.Title,
            Message = notification.Message,
            SequenceNumber = notification.SequenceNumber
        };
    }
}