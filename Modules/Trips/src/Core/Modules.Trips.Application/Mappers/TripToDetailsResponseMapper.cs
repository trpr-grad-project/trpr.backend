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
                ThemeId = source.ThemeId,
                Title = source.Title,
                Description = source.Description,
                Price = source.Price,
                ExpectedDuration = source.ExpectedDuration,
                ImagesUrls = imagePaths,
                TripVisibility = source.TripVisibility,
                Status = source.Status,
                PublishMode = source.PublishMode,
                RejectionReason = source.RejectionReason,
                Segments = [.. source.Segments
                    .SelectMany(s => s.Places)
                    .Select(p => p.ToPlaceDto())],
                MaxParticipantsCount = source.MaxParticipantsCount,
                CreatedAtUTC = source.CreatedAtUTC
            };
        }
    }
}
