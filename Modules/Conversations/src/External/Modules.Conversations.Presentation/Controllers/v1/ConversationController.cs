using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Conversations.Application.Dtos.Requests;
using Modules.Conversations.Application.Interfaces;

namespace Modules.Conversations.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/conversation")]
    public class ConversationController(IAiChatService aiChatService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();

        [Authorize]
        [HttpPost("ai")]
        public async Task<IActionResult> GetPrompt(SendAiPromptRequestDto request)
        {
            var result = await aiChatService.SendMessageAsync(UserId, request);
            return Ok(result);
        }
        [HttpPost("direct")]
        public Task<IActionResult> SendDirectMessage(SendDirectMessageRequestDto request)
        {
            // var result = await aiChatService.SendDirectMessageAsync(request);
            // return Ok(result);
            throw new NotImplementedException("Direct messaging is not implemented yet.");
        }
        [HttpPost("{Id}")]
        public Task<IActionResult> SendMessageToConversation(Guid Id, string message)
        {
            throw new NotImplementedException("Sending messages to existing conversations is not implemented yet.");
        }
    }
}