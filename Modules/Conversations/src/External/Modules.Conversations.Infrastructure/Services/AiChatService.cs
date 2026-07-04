using System.Formats.Asn1;
using System.Security.Cryptography.X509Certificates;
using Common.Application.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Modules.Conversations.Application.Abstractions;
using Modules.Conversations.Application.Dtos.Requests;
using Modules.Conversations.Application.Dtos.Responses;
using Modules.Conversations.Application.Interfaces;
using Modules.Conversations.Domain.Entities;
using Modules.Conversations.Infrastructure.Mappers;
using Newtonsoft.Json;

namespace Modules.Conversations.Infrastructure.Services
{
    public class AiChatService(IChatClient chatClient, RepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : IAiChatService
    {
        private readonly ChatMessage systemPrompt = new ChatMessage(ChatRole.System, """
            You are a helpful AI assistant. 
            - Always be concise and polite in your responses. 
            """);
        static string GetWeather(string city)
        {
            return $"The weather in {city} is sunny.";
        }
        static ICollection<string> GetEvents(string city)
        {
            return
            [
                $"Tech Conference 2026 in {city}",
                $"Music Festival in {city} Downtown",
                $"Startup Networking Meetup in {city}",
                $"Food Expo at {city} International Center",
                $"AI & Cloud Summit in {city}"
            ];
        }

        public async Task<MessageResponseDto> SendMessageAsync(Guid userId, SendAiPromptRequestDto request, CancellationToken cancellationToken = default)
        {
            var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(
                x => x.Id == userId
            ) ?? throw new NotFoundException("User.NotFound");

            AiConversation conversation =
                request.ConversationId == null ? AiConversation.Create(userId) : await repositoryFactory
                .Repository<AiConversation>()
                .GetQueryable()
                .Include(x => x.Messages)
                .Where(x => x.Id == request.ConversationId && x.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                ?? throw new NotFoundException("Conversation.NotFound");

            ICollection<ChatMessage> chatHistory = conversation
                .Messages
                .OrderBy(x => x.Id)
                .Select(x => x.ToChatMessage(conversation))
                .ToList();

            var userPrompt = new ChatMessage(ChatRole.User, request.Prompt);

            var messagesToBeSent =
                new List<ChatMessage> { systemPrompt }
                .Concat(chatHistory)
                .Concat([userPrompt]);

            ChatResponse response = await chatClient.GetResponseAsync(
                messages: messagesToBeSent,
                options: GenerateChatOptions(),
                cancellationToken: cancellationToken);
            var userMessage = userPrompt.ToAiMessage(conversation);
            var aiMessage = response.Messages.Last().ToAiMessage(conversation);

            if (request.ConversationId == null)
                repositoryFactory.Repository<AiConversation>().Add(conversation);
            else
                repositoryFactory.Repository<AiConversation>().Attach(conversation);
            repositoryFactory.Repository<AiMessage>().Add(userMessage);
            repositoryFactory.Repository<AiMessage>().Add(aiMessage);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return new MessageResponseDto
            {
                Id = aiMessage.Id,
                ConversationId = aiMessage.ConversationId,
                SenderUserId = null,
                Content = aiMessage.Contnet
            };

        }
        private static ChatOptions GenerateChatOptions()
        {
            return new ChatOptions
            {
                Tools = [
                    AIFunctionFactory.Create(
                        name : "get_weather",
                        description : "Get the current weather for a specific city by name.",
                        method : GetWeather
                    ),
                    AIFunctionFactory.Create(
                        name : "get_events",
                        description : "Get a list of upcoming events in a specific city.",
                        method : GetEvents
                    )],
                ToolMode = ChatToolMode.Auto,
            };
        }
    }
}