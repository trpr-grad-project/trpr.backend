using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Helpers;

public static class ProfileMapper
{
    public static ProfileResponseDto ToProfileResponseDto(Profile profile)
    {
        return new ProfileResponseDto
        {
            Id = profile.Id,
            Languages = ToLanguageResponseDtos(profile.Languages),
            Interests = ToInterestResponseDtos(profile.Interests),
            Vibes = ToVibeResponseDtos(profile.Vibes)
        };
    }

    private static List<LanguageResponseDto> ToLanguageResponseDtos(ICollection<ProfileLanguage> profileLanguages)
    {
        return profileLanguages
            .Select(pl => new LanguageResponseDto
            {
                Id = pl.Language.Id,
                Name = pl.Language.Name,
                Code = pl.Language.Code,
                NativeName = pl.Language.NativeName,
                Icon = pl.Language.Icon
            })
            .ToList();
    }

    private static List<InterestResponseDto> ToInterestResponseDtos(ICollection<ProfileInterest> profileInterests)
    {
        return profileInterests
            .Select(pi => new InterestResponseDto
            {
                Id = pi.Interest.Id,
                Icon = pi.Interest.Icon,
                Name = pi.Interest.Name
            })
            .ToList();
    }

    private static List<VibeResponseDto> ToVibeResponseDtos(ICollection<ProfileVibe> profileVibes)
    {
        return profileVibes
            .Select(pv => new VibeResponseDto
            {
                Id = pv.Vibe.Id,
                Thumbnail = pv.Vibe.Thumbnail,
                Description = pv.Vibe.Description,
                Name = pv.Vibe.Name,
                IsActive = pv.Vibe.IsActive
            })
            .ToList();
    }
}
