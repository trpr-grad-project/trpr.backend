using Common.Application;
using Common.Application.Buckets;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Domain.Entities;


namespace Modules.Trips.Application.Mappers
{
    public class TripToResponseMapper(IFileService fileService) : IMapper<Trip, TripResponseDto>
    {
        public TripResponseDto Map(Trip source)
        {
            ICollection<string> imagePaths = fileService.ResolveUrls(source.Images);
            TripResponseDto responseDto = new()
            {
                CreatedByUser = source.UserId,
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
                Segments = [.. source.Segments
                    .SelectMany(s => s.Places)
                    .Select(x => x.ToPlaceDto())],
                MaxParticipantsCount = source.MaxParticipantsCount,
                RejectionReason = source.RejectionReason
            };
            return responseDto;
        }
    }
}
