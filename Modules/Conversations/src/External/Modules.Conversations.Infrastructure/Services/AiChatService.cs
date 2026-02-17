using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using Modules.Conversations.Application.Interfaces;

namespace Modules.Conversations.Infrastructure.Services
{
    public class AiChatService(IChatClient chatClient) : IAiChatService
    {
        private readonly ChatMessage systemPrompt = new ChatMessage(ChatRole.System, """
            You are a helpful AI assistant. 
            - When the user asks about the weather in a specific city, call the "get_weather" function with the city name. 
            - If the user asks unrelated questions, answer them directly. 
            - Always be concise and polite in your responses. 
            """);
        static string GetWeather(string city)
        {
            return $"The weather in {city} is sunny.";
        }

        public async Task<ICollection<KeyValuePair<string, object?>>> SendMessageAsync(string message)
        {
            ChatResponse response = await chatClient.GetResponseAsync(
                messages: [systemPrompt, new ChatMessage(ChatRole.User, message)],
                options: new ChatOptions
                {
                    ModelId = "gemini-3-flash-preview",
                    Temperature = 0.7f,
                    TopP = 0.9f,
                    Tools = [AIFunctionFactory.Create(
                        name : "get_weather",
                        description : "Get the current weather for a specific city by name.",
                        method : GetWeather
                    )],
                    ToolMode = ChatToolMode.Auto,
                });
            return response.Messages.Select(m => new KeyValuePair<string, object?>(m.Role.ToString(), m.Contents)).ToList();
        }
    }
}