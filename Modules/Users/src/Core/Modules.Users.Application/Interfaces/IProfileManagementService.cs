using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;

namespace Modules.Users.Application.Interfaces;

public interface IProfileManagementService
{
    /// <summary>
    /// Create a new profile for a user with languages, interests, and vibes
    /// </summary>
    Task<ProfileResponseDto> CreateProfileAsync(Guid userId, CreateProfileRequestDto createRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get profile by user ID with all related languages, interests, and vibes
    /// </summary>
    Task<ProfileResponseDto> GetProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update profile with languages, interests, and vibes
    /// </summary>
    Task<ProfileResponseDto> UpdateProfileAsync(Guid userId, UpdateProfileBulkRequestDto updateRequest, CancellationToken cancellationToken = default);

}
