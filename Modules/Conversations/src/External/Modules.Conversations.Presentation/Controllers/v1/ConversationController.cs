using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Conversations.Application.Dtos.Requests;
using Modules.Conversations.Application.Dtos.Responses;
using Modules.Conversations.Application.Interfaces;
using Modules.Conversations.Application.Services;

namespace Modules.Conversations.Presentation.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Route("api/v1/conversations")]
    public class ConversationController(IAiChatService aiChatService, ChatService chatService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();

        [HttpPost("ai")]
        public async Task<ActionResult<MessageResponseDto>> GetPrompt(SendAiPromptRequestDto request)
        {
            MessageResponseDto result = await aiChatService.SendMessageAsync(UserId, request);
            return Ok(result);
        }
        [HttpPost("{id}")]
        public async Task<ActionResult<MessageListItemDto>> SendMessageToConversation(Guid id, SendMessageRequestDto request)
        {
            MessageListItemDto result = await chatService.SendMessage(UserId, id, request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ConversationDetailsResponseDto>> CreateConversation(CreateConversationRequestDto request)
        {
            var result = await chatService.CreateConversation(UserId, request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<ConversationCursorPageDto>> GetConversations([FromQuery] GetConversationsQueryDto query)
        {
            var result = await chatService.GetConversationsAsync(UserId, query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConversationDetailsResponseDto>> GetConversation(Guid id)
        {
            var result = await chatService.GetConversationDetailsAsync(UserId, id);
            return Ok(result);
        }

        [HttpGet("{id}/messages")]
        public async Task<ActionResult<MessageCursorPageDto>> GetConversationMessages(Guid id, [FromQuery] GetConversationMessagesQueryDto query)
        {
            var result = await chatService.GetConversationMessagesAsync(UserId, id, query);
            return Ok(result);
        }

    }
}