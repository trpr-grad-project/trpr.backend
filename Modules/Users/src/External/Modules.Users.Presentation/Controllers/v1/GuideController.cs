using Common.Application.Buckets;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Services;
using Modules.Users.Domain.Entities;

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

        [HttpPost("request")]
        public async Task<ActionResult<GuideUpgradeResponseDto>> UpgradeRequest(GuideUpgradeRequestDto dto, CancellationToken cancellationToken)
        {
            ActionResult<GuideUpgradeResponseDto> request = await GuideService.UpgradeToGuide(UserId, dto, cancellationToken);
            return Ok(request);
        }
    }
}