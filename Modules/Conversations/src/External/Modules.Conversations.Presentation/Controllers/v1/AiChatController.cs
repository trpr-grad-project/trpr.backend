using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Conversations.Application.Interfaces;

namespace Modules.Conversations.Presentation.Controllers.v1
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AiChatController(IAiChatService aiChatService) : ControllerBase
    {
        [HttpGet("{prompt}")]
        public async Task<IActionResult> GetPrompt(string prompt)
        {
            var result = await aiChatService.SendMessageAsync(prompt);
            return Ok(result);
        }
    }
}