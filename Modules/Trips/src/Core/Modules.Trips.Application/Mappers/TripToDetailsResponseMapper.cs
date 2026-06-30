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
                CreatedByUser = new UserResponseDto
                {
                    Id = source.CreatedByUser.Id,
                    UserName = source.CreatedByUser.UserName,
                    FirstName = source.CreatedByUser.FirstName,
                    LastName = source.CreatedByUser.LastName,
                    Email = source.CreatedByUser.Email,
                    PhoneNumber = source.CreatedByUser.PhoneNumber,
                    Rating = source.CreatedByUser.Rating,
                },
                AutoApprove = source.AutoApprove,
                GuideId = source.GuideId,
                Theme = source.TripTheme.Name,
                CreatorRoles = Enum.GetValues<UserRole>()
                    .Where(r => source.CreatorRole.HasFlag(r))
                    .Select(r => r.ToString())
                    .ToList(),
                Title = source.Title,
                Description = source.Description,
                Price = source.Price,
                ImagesUrls = imagePaths,
                TripVisibility = source.TripVisibility,
                Status = source.Status,
                PublishMode = source.PublishMode,
                RejectionReason = source.RejectionReason,
                StartDate = source.StartDate,
                TripTime = source.Segments.Max(x => x.Order).ToString() + "Day(s)",
                PendingParticipants = source.Participants
                    .Where(p => p.Approved == false)
                    .Select(p => new UserResponseDto
                    {
                        Id = p.User.Id,
                        UserName = p.User.UserName,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        Email = p.User.Email,
                        PhoneNumber = p.User.PhoneNumber,
                        Rating = p.User.Rating,
                    }).ToList(),
                ApprovedParticipants = source.Participants
                    .Where(p => p.Approved == true)
                    .Select(p => new UserResponseDto
                    {
                        Id = p.User.Id,
                        UserName = p.User.UserName,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        Email = p.User.Email,
                        PhoneNumber = p.User.PhoneNumber,
                        Rating = p.User.Rating,
                    }).ToList(),
                Segments = source
                    .Segments
                    .OrderBy(x => x.Order)
                    .Select((x, idx) => new DayResponseDto
                    {
                        Day = idx + 1,
                        Duration = x.Duration,
                        Places = x.Places.Select(p => p.ToPlaceDto()).ToList()
                    }).ToList(),
                MaxParticipantsCount = source.MaxParticipantsCount,
                CreatedAtUTC = source.CreatedAtUTC
            };
        }
    }
}
