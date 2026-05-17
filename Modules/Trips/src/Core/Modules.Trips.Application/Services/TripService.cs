using Common.Application;
using Common.Application.Buckets;
using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Trips.Application.Abstractions;
using Modules.Trips.Application.Dtos;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Services
{
    public class TripService(IUnitOfWork unitOfWork,
        RepositoryFactory repositoryFactory,
        PlaceService placeService,
        IMapper<Trip, TripResponseDto> tripMapper)
    {
        public async Task<TripResponseDto> CreateTrip(CreateTripRequestDto dto, Guid userId, CancellationToken cancellationToken)
        {
            var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(User => User.Id == userId)
                ?? throw new NotFoundException("User.NotFound", userId);
            var segments = await GetPlacesAsync(dto.Segments);
            var governorates = segments
                .SelectMany(x => x)
                .Select(x => x.Governorate)
                .DistinctBy(x => x.Id)
                .ToList();
            var trip = Trip.Create(
                        userId,
                        dto.ThemeId,
                        dto.Title,
                        dto.Description,
                        dto.Price,
                        dto.Images,
                        dto.TripVisibility,
                        segments,
                        dto.MaxParticipantsCount,
                        dto.GuideId,
                        dto.Segments.Select(s => s.Duration).ToList(),
                        user,
                        governorates);
            repositoryFactory.Repository<Trip>().Add(trip);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return tripMapper.Map(trip);
        }

        public async Task<ICollection<TripResponseDto>> GetTrips(SearchTripRequestDto request, CancellationToken cancellationToken)
        {
            // TODO FILTER BASED ON THE SEARCH REQUEST LATER
            var trips = await repositoryFactory.Repository<Trip>().GetQueryable()
                .Include(x => x.Segments).ThenInclude(s => s.Places)
                .Include(x => x.Images)
                .Include(x => x.CreatedByUser)
                .ToListAsync(cancellationToken);
            return trips.Select(tripMapper.Map).ToList();
        }
        #region Helpers
        private async Task<ICollection<ICollection<Place>>> GetPlacesAsync(ICollection<DayDto> source)
        {
            ICollection<ICollection<Place>> places = [];
            foreach (var day in source)
            {
                ICollection<Place> dayPlaces = await placeService.GetPlacesAsync(day.PlacesIds);
                places.Add(dayPlaces);
            }
            return places;
        }
        #endregion
    }
}
