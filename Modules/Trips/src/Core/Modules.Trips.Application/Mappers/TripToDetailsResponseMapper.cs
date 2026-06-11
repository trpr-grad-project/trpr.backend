using Common.Application;
using Common.Application.Buckets;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Mappers
{
    public class TripToDetailsResponseMapper(IFileService fileService) : IMapper<Trip, TripDetailsResponseDto>
    {
        public TripDetailsResponseDto Map(Trip source)
        {
            ICollection<string> imagePaths = fileService.ResolveUrls(source.Images);
            return new TripDetailsResponseDto
            {
                Id = source.Id,
                CreatedByUserId = source.UserId,
                GuideId = source.GuideId,
                ThemeId = source.TripTheme.Id,
                CreatorRoles = Enum.GetValues<UserRole>()
                    .Where(r => source.CreatorRole.HasFlag(r))
                    .Select(r => r.ToString())
                    .ToList(),
                Title = source.Title,
                Description = source.Description,
                Price = source.Price,
                ExpectedDuration = source.ExpectedDuration,
                ImagesUrls = imagePaths,
                TripVisibility = source.TripVisibility,
                Status = source.Status,
                PublishMode = source.PublishMode,
                RejectionReason = source.RejectionReason,
                Segments = source
                    .Segments
                    .OrderBy(x => x.Order)
                    .Select((x, idx) => new DayResponseDto
                    {
                        Day = idx + 1,
                        Places = x.Places.Select(p => p.ToPlaceDto()).ToList()
                    }).ToList(),
                MaxParticipantsCount = source.MaxParticipantsCount,
                CreatedAtUTC = source.CreatedAtUTC
            };
        }
    }
}
