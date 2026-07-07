using Modules.Notifications.Contracts.Contracts;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Helpers;

public static class ProfileMapper
{
    public static ProfileResponseDto ToProfileResponseDto(Profile profile, string avatar)
    {
        return new ProfileResponseDto
        {
            Id = profile.Id,
            Bio = profile.Bio,
            Languages = [.. profile.Languages.Select(x => x.Language.ToLanguageResponseDto())],
            Interests = [.. profile.Interests.Select(x => x.Interest.ToInterestResponseDto())],
            Vibes = [.. profile.Vibes.Select(x => x.Vibe.ToVibeResponseDto())],
            AvatarUrl = avatar,
            Reviews = profile.Reviews,
            Rating = profile.Rating,
        };
    }

    public static LanguageResponseDto ToLanguageResponseDto(this Language language)
    {
        return new LanguageResponseDto
        {
            Id = language.Id,
            Name = language.Name,
            Code = language.Code,
            NativeName = language.NativeName,
            Icon = language.Icon
        };
    }

    public static InterestResponseDto ToInterestResponseDto(this Interest interest)
    {
        return new InterestResponseDto
        {
            Id = interest.Id,
            Icon = interest.Icon,
            Name = interest.Name
        };
    }

    public static VibeResponseDto ToVibeResponseDto(this Vibe vibe)
    {
        return new VibeResponseDto
        {
            Id = vibe.Id,
            Thumbnail = vibe.Thumbnail,
            Description = vibe.Description,
            Name = vibe.Name,
        };
    }


}
