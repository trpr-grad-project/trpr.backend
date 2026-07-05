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
                TripId = source.Id,
                CreatedByUser = source.UserId,
                GuideId = source.GuideId,
                Theme = source.TripTheme.Name,
                CreatorRoles = Enum.GetValues<UserRole>()
                    .Where(r => source.CreatorRole.HasFlag(r))
                    .Select(r => r.ToString())
                    .ToList(),
                Title = source.Title,
                StartDate = source.StartDate,
                Description = source.Description,
                Price = source.Price,
                ImagesUrls = imagePaths,
                TripTime = source.Segments.Count.ToString() + " Day(s)",
                TripVisibility = source.TripVisibility,
                EndDate = source.EndDate,
                Status = source.Status,
                AutoApprove = source.AutoApprove,
                Segments = source
                    .Segments
                    .OrderBy(x => x.Order)
                    .Select((x, idx) => new DayResponseDto
                    {
                        Day = idx + 1,
                        Duration = x.Duration,
                        DayDate = x.DayDate,
                        Places = x.Places.Select(p => p.ToPlaceDto()).ToList()
                    }).ToList(),
                MaxParticipantsCount = source.MaxParticipantsCount,
                RejectionReason = source.RejectionReason
            };
            return responseDto;
        }
    }
}
