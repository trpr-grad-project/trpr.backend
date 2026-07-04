using Common.Application.Serialization;
using Microsoft.Extensions.AI;
using Modules.Conversations.Domain.Entities;
using Modules.Conversations.Domain.ValueObjects;
using Newtonsoft.Json;

namespace Modules.Conversations.Infrastructure.Mappers
{
    public static class AiMessageMapper
    {
        public static AiMessage ToAiMessage(this ChatMessage userPrompt, AiConversation conversation)
        {
            var userPromptContent = userPrompt.Text;
            AiMessageRole role = AiMessageRole.User;
            if (userPrompt.Role == ChatRole.User)
                role = AiMessageRole.User;
            if (userPrompt.Role == ChatRole.Assistant)
                role = AiMessageRole.Assistant;
            return AiMessage.Create(conversation, userPromptContent, role);
        }

        public static ChatMessage ToChatMessage(this AiMessage aiMessage, AiConversation conversation)
        {
            var aiMessageContent = aiMessage.Contnet;
            ChatRole role = ChatRole.User;
            if (aiMessage.Role == AiMessageRole.User)
                role = ChatRole.User;
            if (aiMessage.Role == AiMessageRole.Assistant)
                role = ChatRole.Assistant;
            if (aiMessage.Role == AiMessageRole.System)
                role = ChatRole.System;
            return new ChatMessage(role, aiMessageContent);
        }
    }
}