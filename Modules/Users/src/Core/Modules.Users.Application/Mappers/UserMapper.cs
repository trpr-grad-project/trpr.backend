using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Mappers
{
    public static class UserMapper
    {
        public static UserResponseDto ToResponseDto(this User user)
        {
            if (user.Profile != null)
            {
                var profileDto = new ProfileResponseDto
                {
                    Id = user.Profile.Id,
                    Languages = user.Profile.Languages.Select(pl => new LanguageResponseDto
                    {
                        Id = pl.Language.Id,
                        Name = pl.Language.Name,
                        Code = pl.Language.Code,
                        NativeName = pl.Language.NativeName,
                        Icon = pl.Language.Icon
                    }).ToList(),
                    Interests = user.Profile.Interests.Select(pi => new InterestResponseDto
                    {
                        Id = pi.Interest.Id,
                        Icon = pi.Interest.Icon,
                        Name = pi.Interest.Name
                    }).ToList(),
                    Vibes = user.Profile.Vibes.Select(pv => new VibeResponseDto
                    {
                        Id = pv.Vibe.Id,
                        Thumbnail = pv.Vibe.Thumbnail,
                        Description = pv.Vibe.Description,
                        Name = pv.Vibe.Name
                    }).ToList(),
                    Rating = user.Profile.Rating,
                    Reviews = user.Profile.Reviews,
                };
                return new UserResponseDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Profile = profileDto,
                };
            }
            return new UserResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }
    }
}
