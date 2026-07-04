using Common.Application.Dtos;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Notifications.Application.Dtos.Requests;
using Modules.Notifications.Application.Dtos.Responses;
using Modules.Notifications.Application.Services;

namespace Modules.Notifications.Presentation.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v1/notifications")]
    public class NotificationController(NotificationService notificationService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();
        [HttpGet]
        public async Task<ActionResult<CursorPageDto<NotificationResponseDto, Guid?>>> Get([FromQuery] QueryNotificationsRequestDto request)
        {
            var result = await notificationService.GetNotificationsAsync(UserId, request);
            return Ok(result);
        }
    }

}