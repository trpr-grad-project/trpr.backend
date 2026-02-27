using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Modules.Conversations.Domain.Entities;
using Newtonsoft.Json;

namespace Modules.Conversations.Infrastructure.Mappers
{
    public static class AiMessageMapper
    {
        public static AiMessage ToAiMessage(this ChatMessage userPrompt, AiConversation conversation, ICollection<ChatMessage>? responses = null)
        {
            responses ??= [];
            var userPromptContent = JsonConvert.SerializeObject(userPrompt, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            })!;

            ICollection<string> responsesChatContent = [..
                responses
                .Select(x =>
                    JsonConvert.SerializeObject(x, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })!
                )];

            return AiMessage.Create(conversation, userPromptContent, responsesChatContent);
        }

        public static ICollection<ChatMessage> ToChatMessage(this ICollection<AiMessage> aiMessages)
        {
            return [.. aiMessages
                .Select(x => x.ToChatMessage())];
        }

        public static ChatMessage ToChatMessage(this AiMessage aiMessage)
        {
            return JsonConvert.DeserializeObject<ChatMessage>(aiMessage.Contnet)!;
        }
    }
}