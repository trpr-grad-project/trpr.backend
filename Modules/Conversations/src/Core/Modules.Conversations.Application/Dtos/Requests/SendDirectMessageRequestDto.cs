using Modules.Conversations.Domain.ValueObjects;

namespace Modules.Conversations.Application.Dtos.Requests;

public class SendDirectMessageRequestDto
{
    public string MessageContent { get; set; } = string.Empty;
    public Guid RecipientId { get; set; }
}