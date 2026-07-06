using Common.Application.Buckets;
using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Notifications.Contracts.Contracts;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Helpers;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Services;

public class ProfileManagementService(
    IUsersDbContext context,
    IUnitOfWork unitOfWork,
    ILogger<ProfileManagementService> logger,
    IFileService fileService)
{
    public async Task<ProfileResponseDto> CreateProfileAsync(Guid userId, CreateProfileRequestDto createRequest, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating profile for user {UserId}", userId);

        var existingProfile = await context.Profiles
            .FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);

        if (existingProfile != null)
        {
            logger.LogWarning("Profile already exists for user {UserId}", userId);
            throw new ConflictException("Profile.Conflict");
        }

        await ValidateReferences(createRequest);
        var profile = new Profile { Id = userId };
        UpdateProfileLanguages(profile, createRequest.LanguageIds);
        UpdateProfileInterests(profile, createRequest.InterestIds);
        UpdateProfileVibes(profile, createRequest.VibeIds);
        if (!string.IsNullOrEmpty(createRequest.Bio))
        {
            profile.Bio = createRequest.Bio;
        }
        if (!string.IsNullOrEmpty(createRequest.AvatarUrl))
        {
            profile.AvatarUrl = createRequest.AvatarUrl;
        }
        context.Profiles.Add(profile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Profile created successfully for user {UserId}", userId);

        return await GetProfileByUserIdAsync(userId, cancellationToken);
    }

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
            throw new NotFoundException("Profile.NotFound");
        }
        
        return ProfileMapper.ToProfileResponseDto(profile, fileService.ResolveUrl(profile.AvatarUrl));
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
            throw new NotFoundException("Profile.NotFound");
        }

        // Update languages
        UpdateProfileLanguages(profile, updateRequest.LanguageIds);
        UpdateProfileInterests(profile, updateRequest.InterestIds);
        UpdateProfileVibes(profile, updateRequest.VibeIds);
        if(!string.IsNullOrEmpty(updateRequest.Bio))
        {
            profile.Bio = updateRequest.Bio;
        }
        if(!string.IsNullOrEmpty(updateRequest.AvatarUrl))
        {
            profile.AvatarUrl = updateRequest.AvatarUrl;
        }
        context.Profiles.Update(profile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Profile updated successfully for user {UserId}", userId);

        // Reload the profile with all related data
        return await GetProfileByUserIdAsync(userId, cancellationToken);
        
    }

    private static void UpdateProfileLanguages(Profile profile, List<int>? languageIds)
    {
        if (languageIds != null)
        {
            profile.Languages.Clear();
            foreach (var languageId in languageIds)
            {
                profile.Languages.Add(new ProfileLanguage { ProfileId = profile.Id, LanguageId = languageId });
            }
        }
    }

    private static void UpdateProfileInterests(Profile profile, List<int>? interestIds)
    {
        if (interestIds != null)
        {
            profile.Interests.Clear();
            foreach (var interestId in interestIds)
            {
                profile.Interests.Add(new ProfileInterest { ProfileId = profile.Id, InterestId = interestId });
            }
        }
    }

    private static void UpdateProfileVibes(Profile profile, List<int>? vibeIds)
    {
        if (vibeIds != null)
        {
            profile.Vibes.Clear();
            foreach (var vibeId in vibeIds)
            {
                profile.Vibes.Add(new ProfileVibe { ProfileId = profile.Id, VibeId = vibeId });
            }
        }
    }

    private async Task ValidateReferences(CreateProfileRequestDto dto)
    {
        var langIds = dto.LanguageIds ?? [];
        logger.LogInformation("Validating languages: {LanguageIds}", langIds);
        var languagesExist = await context.Languages
            .CountAsync(l => langIds.Contains(l.Id));
        logger.LogInformation("Languages found: {LanguagesExist}", languagesExist);
        if (languagesExist != langIds.Count)
            throw new NotFoundException("Language.Ref.NotFound");

        var intrestIds = dto.InterestIds ?? [];
        logger.LogInformation("Validating interests: {InterestIds}", intrestIds);
        var interestsExist = await context.Interests
            .CountAsync(i => intrestIds.Contains(i.Id));
        logger.LogInformation("Interests found: {InterestsExist}", interestsExist);
        if (interestsExist != intrestIds.Count)
            throw new NotFoundException("Interest.Ref.NotFound");

        var vibeIds = dto.VibeIds ?? [];
        logger.LogInformation("Validating vibes: {VibeIds}", vibeIds);
        var vibesExist = await context.Vibes
            .CountAsync(v => vibeIds.Contains(v.Id));
        logger.LogInformation("Vibes found: {VibesExist}", vibesExist);
        if (vibesExist != vibeIds.Count)
            throw new NotFoundException("Vibe.Ref.NotFound");
    }

    public async Task<ProfileLookupResponseDto> GetProfileMetaDataAsync(CancellationToken cancellationToken = default)
    {
        var vibesTask = await context.Vibes
            .AsNoTracking()
            .Select(v => v.ToVibeResponseDto())
            .ToListAsync(cancellationToken);
        var interestsTask = await context.Interests
            .AsNoTracking()
            .Select(i => i.ToInterestResponseDto())
            .ToListAsync(cancellationToken);
        var languagesTask = await context.Languages
            .AsNoTracking()
            .Select(l => l.ToLanguageResponseDto())
            .ToListAsync(cancellationToken);

        return new ProfileLookupResponseDto
        {
            Vibes = vibesTask,
            Interests = interestsTask,
            Languages = languagesTask
        };
    }
}
