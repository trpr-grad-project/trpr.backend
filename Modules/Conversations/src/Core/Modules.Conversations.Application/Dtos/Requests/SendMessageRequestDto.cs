using Modules.Conversations.Domain.ValueObjects;

namespace Modules.Conversations.Application.Dtos.Requests;

public class SendMessageRequestDto
{
    public string MessageContent { get; set; } = string.Empty;
}