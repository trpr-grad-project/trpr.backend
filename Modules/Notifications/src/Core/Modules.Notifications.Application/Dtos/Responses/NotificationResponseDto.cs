namespace Modules.Notifications.Application.Dtos.Responses;

public class NotificationResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int SequenceNumber { get; set; }
}
