using Common.Application.Buckets;
using Common.Application.Dtos;
using Common.Application.Exceptions;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Services;
using Modules.Users.Domain.Entities;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Presentation.Controllers.v1
{
    public class UploadFileDto
    {
        public IFormFile File { get; set; } = default!;
    }
    [ApiController]
    [Route("api/v1/guide")]
    public class GuideController(IFileService fileService, GuideService GuideService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] UploadFileDto request)
        {
            var path = await fileService.UploadFileAsync(request.File);
            var imageUrl = fileService.ResolveUrl(path);
            return Ok(new { Path = imageUrl });
        }

        [HttpPost("upgrade-request")]
        public async Task<ActionResult<GuideUpgradeResponseDto>> UpgradeRequest(GuideUpgradeRequestDto dto, CancellationToken cancellationToken)
        {
            var roles = User.GetRoles();
            foreach(var role in roles)
            {
                if(role == "Guide")
                {
                    throw new NotAuthorizedException("User.AlreadyGuide", UserId);
                }
            }
            ActionResult<GuideUpgradeResponseDto> request = await GuideService.UpgradeToGuide(UserId, dto, cancellationToken);
            return Ok(request);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("requests")]
        public async Task<ActionResult<PaginationDto<UpgradePaginationResponseDto>>> GetUpgradeRequests([FromQuery] UpgradePaginationRequestDto dto, CancellationToken cancellationToken)
        {
            var requests = await GuideService.AllUpgradeRequests(dto, cancellationToken);
            return Ok(requests);
        }
    }
}