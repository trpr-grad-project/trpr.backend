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
using Modules.Trips.Presentation.Controllers.v1;

namespace Modules.Trips.Application.Services
{
    public class TripService(IUnitOfWork unitOfWork,
        RepositoryFactory repositoryFactory,
        PlaceService placeService,
        BiddingService biddingService,
        ITripSuggestionGenerator tripSuggestionGenerator,
        IMapper<Trip, TripResponseDto> tripMapper,
        IMapper<Trip, TripDetailsResponseDto> tripDetailsMapper)
    {
        public async Task<object> GetTripSuggestion(TripSuggestionRequestDto requestDto)
        {
            var places = await placeService.GetPlacesAsync(null, new GetPlacesQueryDto
            {
                GovernorateId = requestDto.GovernorateId,
                Latitude = requestDto.Latitude,
                Longitude = requestDto.Longitude,
                RadiusInMeters = requestDto.RadiusInMeters,
                PageSize = null
            });
            var theme = await repositoryFactory.Repository<Theme>().GetFirstOrDefaultByFilter(t => t.Id == requestDto.ThemeId)
                ?? throw new NotFoundException("Theme.NotFound", requestDto.ThemeId);
            return await tripSuggestionGenerator.GenerateTrip(
                requestDto.StartDateUtc,
                requestDto.NumberOfDays,
                theme, places.Items);
        }
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
            var creatorRoles = roles.Select(x => Enum.Parse<UserRole>(x)).Aggregate(UserRole.User, (a, b) => a | b);
            Theme theme = await repositoryFactory.Repository<Theme>().GetFirstOrDefaultByFilter(t => t.Id == dto.ThemeId)
                ?? throw new NotFoundException("Theme.NotFound", dto.ThemeId);
            var governorates = segments
                .SelectMany(x => x.Value)
                .Select(x => x.Governorate)
                .DistinctBy(x => x.Id)
                .ToList();
            if (creatorRoles.HasFlag(UserRole.Guide) && !dto.GuideId.HasValue)
                dto.GuideId = userId;
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
                        governorates,
                        dto.StartDate);
            repositoryFactory.Repository<Trip>().Add(trip);
            if (!creatorRoles.HasFlag(UserRole.Guide) || !creatorRoles.HasFlag(UserRole.Company))
            {
                var tripParticipant = TripParticipant.Create(trip.Id, userId);
                tripParticipant.Approve();
                repositoryFactory.Repository<TripParticipant>().Add(tripParticipant);
            }
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
        public async Task ApproveTrip(ApproveTripRequestDto dto)
        {
            Trip trip = await repositoryFactory
                .Repository<Trip>()
                .GetFirstOrDefaultByFilter(
                    x => x.Id == dto.TripId && x.UserId == dto.UserId && x.Status == TripStatus.UnderReview)
                ?? throw new NotFoundException("Trip.NotFound");
            trip.Approve();
            await unitOfWork.SaveChangesAsync();
        }
        public async Task JoinTrip(Guid tripId, Guid userId)
        {
            Trip trip = await repositoryFactory
                .Repository<Trip>()
                .GetFirstOrDefaultByFilter(
                    x => x.Id == tripId && x.Status == TripStatus.Published,
                    x => x.Include(x => x.Participants))
                ?? throw new NotFoundException("Trip.NotFound");
            if (trip.MaxParticipantsCount == trip.Participants.Count(x => x.Approved))
            {
                if (trip.Status == TripStatus.Ready)
                    trip.Ready();
                throw new BadRequestException("Trip.MaxParticipantsReached");
            }

            TripParticipant? tripParticipant = trip.Participants
                .FirstOrDefault(x => x.UserId == userId);

            if (tripParticipant != null)
                return;

            User user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(x => x.Id == userId)
                ?? throw new NotFoundException("User.NotFound", userId);

            tripParticipant = TripParticipant.Create(tripId, userId);

            if (trip.AutoApprove)
                tripParticipant.Approve();

            trip.Participants.Add(tripParticipant);
            await unitOfWork.SaveChangesAsync();
            if (trip.MaxParticipantsCount == trip.Participants.Count(x => x.Approved))
            {
                trip.Ready();
            }
        }

        public async Task<ThemeFormDataDto> GetThemeFromData()
        {
            var themes = await repositoryFactory
                .Repository<Theme>()
                .GetQueryable()
                .ToListAsync();
            return new ThemeFormDataDto
            {
                Themes = themes.Select(x => new ThemeDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            };
        }
        // TODO : Enhancement for race condition add for update
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
                if (trip.Status == TripStatus.Ready)
                    trip.Ready();
                throw new BadRequestException("Trip.MaxParticipantsReached");
            }

            TripParticipant tripParticipant = trip.Participants
                .FirstOrDefault(x => x.UserId == userId)
                ?? throw new NotFoundException("TripParticipant.NotFound", userId);

            if (tripParticipant.Approved)
                return;

            tripParticipant.Approve();

            await unitOfWork.SaveChangesAsync();

            if (trip.MaxParticipantsCount == trip.Participants.Count(x => x.Approved))
            {
                await repositoryFactory.Repository<TripParticipant>().GetQueryable()
                    .Where(x => x.TripId == trip.Id && !x.Approved)
                    .ExecuteDeleteAsync();
                trip.Ready();
            }
        }
        public async Task StartTrip(Guid tripId, Guid userId)
        {
            Trip trip = await repositoryFactory
                .Repository<Trip>()
                .GetFirstOrDefaultByFilter(
                    x => x.Id == tripId && x.Status == TripStatus.Ready && x.UserId == userId)
                ?? throw new NotFoundException("Trip.NotFound", tripId);
            if (trip.MaxParticipantsCount == trip.Participants.Count(x => x.Approved))
                trip.Start();
            else
                throw new BadRequestException("Trip.MaxParticipantsNotReached");
            await unitOfWork.SaveChangesAsync();
        }
        public async Task EndTrip(Guid tripId, Guid userId)
        {
            Trip trip = await repositoryFactory
                .Repository<Trip>()
                .GetFirstOrDefaultByFilter(
                    x => x.Id == tripId && x.Status == TripStatus.Started && x.UserId == userId)
                ?? throw new NotFoundException("Trip.NotFound", tripId);
            trip.Complete();
            await unitOfWork.SaveChangesAsync();
        }

        #region Helpers
        private async Task<Dictionary<double, ICollection<Place>>> GetPlacesAsync(ICollection<DayDto> source)
        {
            Dictionary<double, ICollection<Place>> places = [];
            foreach (var day in source)
            {
                ICollection<Place> dayPlaces = await placeService.GetPlacesAsync(day.PlacesIds);
                places.Add(day.Duration, dayPlaces);
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
