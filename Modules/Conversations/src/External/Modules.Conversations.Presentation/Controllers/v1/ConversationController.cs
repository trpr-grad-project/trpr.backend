using System.ComponentModel.DataAnnotations;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Conversations.Application.Dtos.Requests;
using Modules.Conversations.Application.Dtos.Responses;
using Modules.Conversations.Application.Interfaces;
using Modules.Conversations.Application.Services;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/conversation")]
    public class ConversationController(IAiChatService aiChatService, ChatService chatService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();

        [Authorize]
        [HttpPost("ai")]
        public async Task<ActionResult<MessageResponseDto>> GetPrompt(SendAiPromptRequestDto request)
        {
            MessageResponseDto result = await aiChatService.SendMessageAsync(UserId, request);
            return Ok(result);
        }
        [HttpPost("direct")]
        public async Task<ActionResult<MessageResponseDto>> SendDirectMessage(SendMessageRequestDto request)
        {
            MessageResponseDto result = await chatService.StartDirectMessage(UserId, request);
            return Ok(result);
        }
        [HttpPost("conversation")]
        public async Task<ActionResult<MessageResponseDto>> SendMessageToConversation(SendMessageRequestDto request)
        {
            MessageResponseDto result = await chatService.SendMessage(UserId, request);
            return Ok(result);
        }

        [HttpPost("relay")]
        public async Task<ActionResult<ICollection<MessageResponseDto>>> GetRelayMessage([FromBody] GetRelayMessageRequestDto request)
        {
            ICollection<MessageResponseDto> messages = await chatService.GetRelayMessagesAsync(UserId, request.LastConversationsMessages);
            return Ok(messages);
        }

    }
}