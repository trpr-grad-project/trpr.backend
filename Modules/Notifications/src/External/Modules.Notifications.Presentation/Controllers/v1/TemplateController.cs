using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Notifications.Application.Dtos.Requests;
using Modules.Notifications.Application.Services;
using Modules.Notifications.Application.Validators;
using Modules.Notifications.Domain.Entities;
using Modules.Users.Presentation.Extensions;

namespace Modules.Notifications.Presentation.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v1/template")]
    public class TemplateController(TemplateService templateService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTemplate(CreateTemplateDto dto, CancellationToken cancellationToken)
        {
            var id = await templateService.CreateTemplate(UserId, dto, cancellationToken);
            return Ok(id);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Guid>> UpdateTemplate([FromRoute] Guid id, UpdateTemplateDto dto, CancellationToken cancellationToken)
        {
            var Id = await templateService.UpdateTemplate(id, dto, cancellationToken);
            return Ok(Id);
        }

        // TODO: Youssef 
        // add this to the template service and implement the pagination logic 
        // ps. the paginateion dto template has a create method that you can use;
        [HttpGet]
        public async Task<ActionResult<PaginationDto<Template>>> PaginateTemplates([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var templatesList = await templateService.GetTemplatesPagination(page, pageSize, cancellationToken);
            return Ok(templatesList);
        }
    }
}
