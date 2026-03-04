using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Application.Dtos;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Notifications.Application.Dtos.Requests;
using Modules.Notifications.Application.Dtos.Responses;
using Modules.Notifications.Application.Services;
using Modules.Notifications.Application.Validators;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Presentation.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v1/template")]
    public class TemplateController(TemplateService templateService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();
        [HttpPost]
        public async Task<ActionResult<TemplateResponseDto>> CreateTemplate(CreateTemplateDto dto, CancellationToken cancellationToken)
        {
            var id = await templateService.CreateTemplate(UserId, dto, cancellationToken);
            return Ok(id);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<TemplateResponseDto>> UpdateTemplate([FromRoute] Guid id, UpdateTemplateDto dto, CancellationToken cancellationToken)
        {
            var Id = await templateService.UpdateTemplate(id, UserId,dto, cancellationToken);
            return Ok(Id);
        }

        
        [HttpGet]
        public async Task<ActionResult<PaginationDto<TemplatePaginationResponseDto>>> GetPaginatedTemplates([FromQuery] PaginateRequestDto dto, [FromHeader (Name = "X-Language")] string LangCode, CancellationToken cancellationToken = default)
        {
            var templatesList = await templateService.TemplatesPagination(dto, UserId, LangCode,cancellationToken);
            return Ok(templatesList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TemplateResponseDto>> GetTemplateDetails([FromRoute] Guid templateId, CancellationToken cancellationToken = default)
        {
            var template = await templateService.TemplateDetails(templateId, UserId, cancellationToken);
            return Ok(template);
        }
    }
}
