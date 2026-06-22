using Common.Application;
using Common.Application.Dtos;
using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Trips.Application.Abstractions;
using Modules.Trips.Application.Dtos;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Helpers;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;
using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Application.Services
{
    public class TripService(IUnitOfWork unitOfWork,
        RepositoryFactory repositoryFactory,
        PlaceService placeService,
        BiddingService biddingService,
        IMapper<Trip, TripResponseDto> tripMapper,
        IMapper<Trip, TripDetailsResponseDto> tripDetailsMapper)
    {
        public async Task UpdateStatus(UpdateTripStatusRequestDto dto, CancellationToken cancellationToken)
        {
            var trip = await repositoryFactory.Repository<Trip>().GetFirstOrDefaultByFilter(t => t.Id == dto.Id)
                ?? throw new NotFoundException("Trip.NotFound", dto.Id);
            try
            {
                if (dto.IsApproved)
                    trip.Approve();
                else
                    trip.Reject(dto.RejectionReason ?? "No reason provided");
            }
            catch (InvalidOperationException ex)
            {
                throw new ConflictException(ex.Message);
            }
            repositoryFactory.Repository<Trip>().Update(trip);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task<TripResponseDto> CreateTrip(CreateTripRequestDto dto, ICollection<string> roles, Guid userId, CancellationToken cancellationToken)
        {
            var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(User => User.Id == userId)
                ?? throw new NotFoundException("User.NotFound", userId);
            var segments = await GetPlacesAsync(dto.Segments);
            var creatorRoles = roles.Select(x => Enum.Parse<UserRole>(x)).Aggregate((a, b) => a | b);
            Theme theme = await repositoryFactory.Repository<Theme>().GetFirstOrDefaultByFilter(t => t.Id == dto.ThemeId)
                ?? throw new NotFoundException("Theme.NotFound", dto.ThemeId);
            var governorates = segments
                .SelectMany(x => x)
                .Select(x => x.Governorate)
                .DistinctBy(x => x.Id)
                .ToList();
            var trip = Trip.Create(
                        userId,
                        theme,
                        creatorRoles,
                        dto.Title,
                        dto.AutoApprove,
                        dto.Description,
                        dto.Price,
                        dto.Images,
                        dto.TripVisibility,
                        dto.PublishMode,
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

        public async Task<PaginationDto<TripResponseDto>> GetTrips(BaseSearchTripRequestDto request, Guid? userId = null, TripStatus? status = null, CancellationToken cancellationToken = default)
        {
            // TODO FILTER BASED ON THE SEARCH REQUEST LATER
            IQueryable<Trip> trips = repositoryFactory.Repository<Trip>().GetQueryable()
                .Include(x => x.TripTheme)
                .Include(x => x.Segments).ThenInclude(s => s.Places).ThenInclude(p => p.Governorate)
                .Include(x => x.Segments).ThenInclude(s => s.Places).ThenInclude(p => p.Category)
                .Include(x => x.Segments).ThenInclude(s => s.Places).ThenInclude(p => p.PlaceTags).ThenInclude(pt => pt.Tag)
                .Include(x => x.CreatedByUser);
            if (userId.HasValue)
                trips = trips.Where(x => x.UserId == userId.Value);
            if (status.HasValue)
                trips = trips.Where(x => x.Status == status.Value);
            trips = FilterByBaseSearchRequest(trips, request);
            int count = 0;

            ICollection<Trip> pagedTrips = await trips
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            count = await trips.CountAsync(cancellationToken);

            return new PaginationDto<TripResponseDto>
            {
                Items = pagedTrips.Select(tripMapper.Map).ToList(),
                Page = request.Page,
                PageSize = request.PageSize,
                TotalItems = count
            };
        }
        public async Task<TripDetailsResponseDto> GetTripDetailsAsync(Guid tripId, Guid userId, CancellationToken cancellationToken)
        {
            var trip = await repositoryFactory.Repository<Trip>()
                .GetFirstOrDefaultByFilter(
                    t => t.Id == tripId,
                    q => q.Include(t => t.Segments)
                        .ThenInclude(s => s.Places)
                        .ThenInclude(p => p.PlaceTags)
                        .ThenInclude(pt => pt.Tag),
                    q => q.Include(t => t.Segments)
                        .ThenInclude(s => s.Places)
                        .ThenInclude(p => p.Category),
                    q => q.Include(t => t.Segments)
                        .ThenInclude(s => s.Places)
                        .ThenInclude(p => p.Governorate),
                    q => q.Include(t => t.CreatedByUser),
                    q => q.Include(p => p.Participants)
                        .ThenInclude(u => u.User))
                ?? throw new NotFoundException("Trip.NotFound", tripId);

            var dto = tripDetailsMapper.Map(trip);
            if (trip.UserId != userId || trip.AutoApprove == true)
                dto.PendingParticipants = [];
            dto.BiddingsPage =
                (trip.Status == TripStatus.Bidding) ?
                await biddingService.GetTripBiddingsAsync(tripId, userId, new GetTripBiddingsQueryDto
                {
                    PageSize = 10,
                    SortOrder = BiddingSortOrder.Oldest
                }, cancellationToken) :
                null;

            return dto;
        }

        public async Task JoinTrip(Guid tripId, Guid userId)
        {
            Trip trip = await repositoryFactory
                .Repository<Trip>()
                .GetFirstOrDefaultByFilter(
                    x => x.Id == tripId && x.Status == TripStatus.Published,
                    x => x.Include(x => x.Participants))
                ?? throw new NotFoundException("Trip.NotFound");

            TripParticipant? tripParticipant = trip.Participants
                .FirstOrDefault(x => x.UserId == userId);

            if (tripParticipant != null)
                return;

            User user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(x => x.Id == userId)
                ?? throw new NotFoundException("User.NotFound", userId);

            tripParticipant = TripParticipant.Create(tripId, userId);

            if (trip.AutoApprove)
                tripParticipant.Approve();
            if (trip.MaxParticipantsCount == trip.Participants.Count(x => x.Approved))
            {
                trip.Status = TripStatus.Completed;
            }
            trip.Participants.Add(tripParticipant);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ApproveUserJoinRequest(Guid tripId, Guid userId)
        {
            Trip trip = await repositoryFactory
                .Repository<Trip>()
                .GetFirstOrDefaultByFilter(
                    x => x.Id == tripId && x.Status == TripStatus.Published,
                    x => x.Include(x => x.Participants))
                ?? throw new NotFoundException("Trip.NotFound", tripId);

            if (trip.MaxParticipantsCount == trip.Participants.Count(x => x.Approved))
            {
                await repositoryFactory.Repository<TripParticipant>().GetQueryable()
                    .Where(x => x.TripId == trip.Id && !x.Approved)
                    .ExecuteDeleteAsync();
                trip.Status = TripStatus.Completed;
                throw new BadRequestException("Trip.MaxParticipantsReached");
            }

            TripParticipant tripParticipant = trip.Participants
                .FirstOrDefault(x => x.UserId == userId)
                ?? throw new NotFoundException("TripParticipant.NotFound", userId);

            if (tripParticipant.Approved)
                return;

            tripParticipant.Approve();

            if (trip.MaxParticipantsCount == trip.Participants.Count(x => x.Approved))
            {
                await repositoryFactory.Repository<TripParticipant>().GetQueryable()
                    .Where(x => x.TripId == trip.Id && !x.Approved)
                    .ExecuteDeleteAsync();
                trip.Status = TripStatus.Completed;
            }

            await unitOfWork.SaveChangesAsync();
        }
        // create unapproved
        // update trip participants incase of autoapprove = false endpoint
        // make a review endpoint that shows the data of the user who wants to join
        // update getTripDetails endpoint to show the trip participants to trip owner otherwise 
        // keep it the same -- done
        // send notifs to trip creator when a user wants to join
        // send notifs to user when trip creator approves or rejects their request
        // make an endpoint that shows the users who want to join the trip --> in getdetailsasync
        // make an approve enpoint and keep the list of users until max participants is reached, also 
        // update the approved users flag done
        // update trip status to completed if maxcount reached 
        #region Helpers
        private async Task<ICollection<ICollection<Place>>> GetPlacesAsync(ICollection<DayDto> source)
        {
            ICollection<ICollection<Place>> places = new List<ICollection<Place>>();
            foreach (var day in source)
            {
                ICollection<Place> dayPlaces = await placeService.GetPlacesAsync(day.PlacesIds);
                places.Add(dayPlaces);
            }
            return places;
        }

        public static IQueryable<Trip> FilterByBaseSearchRequest(IQueryable<Trip> query, BaseSearchTripRequestDto request)
        {
            if (request.TripType.HasValue)
            {
                if (request.TripType == TripType.ByGuides)
                    query = query.Where(x => (x.CreatorRole & (UserRole.Guide | UserRole.Admin)) == UserRole.Guide);
                else if (request.TripType == TripType.ByCompany)
                    query = query.Where(x => (x.CreatorRole & UserRole.Company) == UserRole.Company);
                else if (request.TripType == TripType.Shared)
                    query = query.Where(x => (x.CreatorRole & UserRole.User) == UserRole.User);
            }

            if (request.Longitude.HasValue && request.Latitude.HasValue && request.RadiusInMeters.HasValue)
            {
                var point = PointUtils
                    .PointFromCoordinates(
                        request.Longitude.Value,
                        request.Latitude.Value);
                query = query.Where(x => x.Centroid.Distance(point) <= request.RadiusInMeters.Value);
            }

            if (request.ThemeId.HasValue)
                query = query.Where(x => x.TripTheme.Id == request.ThemeId.Value);
            if (request.GovernorateId.HasValue)
                query = query.Where(x => x.TripGovernorates.Any(g => g.GovernorateId == request.GovernorateId.Value));
            if (!string.IsNullOrEmpty(request.Title))
                query = query.Where(x => x.Title.Contains(request.Title));
            if (request.MinPrice.HasValue)
                query = query.Where(x => x.Price >= request.MinPrice.Value);
            if (request.MaxPrice.HasValue)
                query = query.Where(x => x.Price <= request.MaxPrice.Value);
            return query;
        }
        #endregion
    }
}
