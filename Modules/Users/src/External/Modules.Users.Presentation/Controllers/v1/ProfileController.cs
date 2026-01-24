using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Services;

namespace Modules.Users.Presentation.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class ProfileController(ProfileManagementService profileManagementService) : ControllerBase
    {
        /// <summary>
        /// Create a new profile for a user with languages, interests, and vibes
        /// </summary>
        [HttpPost("{userId}")]
        public async Task<ActionResult<ProfileResponseDto>> CreateProfile(
            Guid userId,
            [FromBody] CreateProfileRequestDto createRequest,
            CancellationToken cancellationToken)
        {
            var profile = await profileManagementService.CreateProfileAsync(userId, createRequest, cancellationToken);
            return Ok(profile);
        }

        /// <summary>
        /// Get user profile with all languages, interests, and vibes
        /// </summary>
        [HttpGet("{userId}")]
        public async Task<ActionResult<ProfileResponseDto>> GetProfile(Guid userId, CancellationToken cancellationToken)
        {
            var profile = await profileManagementService.GetProfileByUserIdAsync(userId, cancellationToken);
            return Ok(profile);
        }

        /// <summary>
        /// Update profile with languages, interests, and vibes in bulk
        /// </summary>
        [HttpPut("{userId}")]
        public async Task<ActionResult<ProfileResponseDto>> UpdateProfile(
            Guid userId,
            [FromBody] UpdateProfileBulkRequestDto updateRequest,
            CancellationToken cancellationToken)
        {
            var updatedProfile = await profileManagementService.UpdateProfileAsync(userId, updateRequest, cancellationToken);
            return Ok(updatedProfile);
        }

    }
}
