using Common.Application.Dtos;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Services;

namespace Modules.Users.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/support")]
    public class SupportController(SupportService supportService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();

        /// <summary>
        /// Create a new support request with subject and description
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SupportRequestResponseDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<SupportRequestResponseDto>> CreateSupportRequest(
            [FromBody] CreateSupportRequestDto createSupportRequestDto,
            CancellationToken cancellationToken)
        {
            var result = await supportService.CreateSupportRequestAsync(UserId, createSupportRequestDto, cancellationToken);
            return CreatedAtAction(nameof(GetSupportRequest), new { id = result.Id }, result);
        }

        /// <summary>
        /// Get paginated support requests with optional filters
        /// Unread requests appear first, then read requests, ordered by most recent
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(PaginationDto<SupportRequestResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationDto<SupportRequestResponseDto>>> GetSupportRequests(
            [FromQuery] GetSupportRequestsRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var result = await supportService.GetSupportRequestsAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Get a specific support request by ID
        /// Automatically marks the support request as Read when viewed by an admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(SupportRequestResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SupportRequestResponseDto>> GetSupportRequest(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await supportService.GetSupportRequestByIdAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
