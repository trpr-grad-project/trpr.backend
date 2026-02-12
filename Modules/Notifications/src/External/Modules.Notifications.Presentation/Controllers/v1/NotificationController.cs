using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Modules.Notifications.Application.Dtos.Requests;
using Modules.Notifications.Application.Exceptions;
using Modules.Notifications.Application.Services;
using Modules.Notifications.Application.Validators;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/notification")]
    public class NotificationController(NotificationService notificationService) : ControllerBase
    {
        private readonly NotificationService NotificationService = notificationService;

        [HttpPost("create-template")]
        public async Task<ActionResult> CreateTemplate(CreateTemplateDto dto, CancellationToken cancellationToken)
        {
            CreateTemplateDtoValidator validator = new CreateTemplateDtoValidator();   
            var res = validator.Validate(dto);
            if (!res.IsValid) 
            {
                throw new BadRequestException("Wrong input");    
            }
            var id = await NotificationService.CreateTemplate(dto, cancellationToken);
            return Ok();
        } 
    }
}
