using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Exceptions;
using Modules.Users.Application.Helpers;
using Modules.Users.Application.Interfaces;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Services;

public class ProfileManagementService(
    IAppDbContext context,
    IUnitOfWork unitOfWork,
    ILogger<ProfileManagementService> logger) : IProfileManagementService
{
    public async Task<ProfileResponseDto> GetProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching profile for user {UserId}", userId);

        var profile = await context.Profiles
            .AsNoTracking()
            .Include(p => p.Languages)
            .ThenInclude(pl => pl.Language)
            .Include(p => p.Interests)
            .ThenInclude(pi => pi.Interest)
            .Include(p => p.Vibes)
            .ThenInclude(pv => pv.Vibe)
            .FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);

        if (profile == null)
        {
            logger.LogWarning("Profile not found for user {UserId}", userId);
            throw new NotFoundException("Profile.NotFound", userId);
        }

        return ProfileMapper.ToProfileResponseDto(profile);
    }

    public async Task<ProfileResponseDto> UpdateProfileAsync(Guid userId, UpdateProfileBulkRequestDto updateRequest, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Updating profile for user {UserId}", userId);

        var profile = await context.Profiles
            .Include(p => p.Languages)
            .Include(p => p.Interests)
            .Include(p => p.Vibes)
            .FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);

        if (profile == null)
        {
            logger.LogWarning("Profile not found for user {UserId}", userId);
            throw new NotFoundException("Profile.NotFound", userId);
        }

        // Update languages
        UpdateProfileLanguages(profile, userId, updateRequest.LanguageIds);
        UpdateProfileInterests(profile, userId, updateRequest.InterestIds);
        UpdateProfileVibes(profile, userId, updateRequest.VibeIds);
        context.Profiles.Update(profile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Profile updated successfully for user {UserId}", userId);

        // Reload the profile with all related data
        return await GetProfileByUserIdAsync(userId, cancellationToken);
    }

    private void UpdateProfileLanguages(Profile profile, Guid userId, List<Guid>? languageIds)
    {
        if (languageIds != null)
        {
            profile.Languages.Clear();
            foreach (var languageId in languageIds)
            {
                profile.Languages.Add(new ProfileLanguage { ProfileId = userId, LanguageId = languageId });
            }
        }
    }

    private void UpdateProfileInterests(Profile profile, Guid userId, List<Guid>? interestIds)
    {
        if (interestIds != null)
        {
            profile.Interests.Clear();
            foreach (var interestId in interestIds)
            {
                profile.Interests.Add(new ProfileInterest { ProfileId = userId, InterestId = interestId });
            }
        }
    }

    private void UpdateProfileVibes(Profile profile, Guid userId, List<Guid>? vibeIds)
    {
        if (vibeIds != null)
        {
            profile.Vibes.Clear();
            foreach (var vibeId in vibeIds)
            {
                profile.Vibes.Add(new ProfileVibe { ProfileId = userId, VibeId = vibeId });
            }
        }
    }
}
