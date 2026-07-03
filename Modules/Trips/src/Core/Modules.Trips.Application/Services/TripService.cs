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
                ?? throw new NotFoundException("Theme.NotFound");
            return await tripSuggestionGenerator.GenerateTrip(
                requestDto.StartDateUtc,
                requestDto.NumberOfDays,
                theme, places.Items);
        }
        public async Task UpdateStatus(UpdateTripStatusRequestDto dto, CancellationToken cancellationToken)
        {
            var trip = await repositoryFactory.Repository<Trip>().GetFirstOrDefaultByFilter(t => t.Id == dto.Id)
                ?? throw new NotFoundException("Trip.NotFound");
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
        public async Task<TripResponseDto> CreateTripByCompany(CompanyCreateTripRequestDto dto, ICollection<string> roles, Guid userId, CancellationToken cancellationToken)
        {
            var trip = await CreateTripCore(dto, roles, userId,
                TripVisibility.Public, TripPublishMode.DirectPublish,
                guideId: dto.GuideId,
                notFoundKey: "Company.NotFound",
                cancellationToken);
            await UpdateStatus(new UpdateTripStatusRequestDto { Id = trip.Id, IsApproved = true }, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return tripMapper.Map(trip);
        }

        public async Task<TripResponseDto> CreateTripByGuide(CreateTripRequestDto dto, ICollection<string> roles, Guid userId, CancellationToken cancellationToken)
        {
            // the creator is the guide
            var trip = await CreateTripCore(dto, roles, userId,
                TripVisibility.Public, TripPublishMode.DirectPublish,
                guideId: userId,
                notFoundKey: "Guide.NotFound",
                cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return tripMapper.Map(trip);
        }
        public async Task<TripResponseDto> CreateTripByUser(UserCreateTripRequestDto dto, ICollection<string> roles, Guid userId, CancellationToken cancellationToken)
        {
            var trip = await CreateTripCore(dto, roles, userId,
                dto.TripVisibility, dto.PublishMode,
                guideId: dto.GuideId,
                notFoundKey: "User.NotFound",
                cancellationToken);
            var tripParticipant = TripParticipant.Create(trip.Id, userId);
            tripParticipant.Approve();
            repositoryFactory.Repository<TripParticipant>().Add(tripParticipant);

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
                trips = trips.Where(x =>
                    x.UserId == userId.Value ||
                    x.Participants.Any(p => p.UserId == userId.Value)
                );
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
                        .ThenInclude(u => u.User),
                    q => q.Include(th => th.TripTheme))
                ?? throw new NotFoundException("Trip.NotFound");

            var dto = tripDetailsMapper.Map(trip);
            if (trip.UserId != userId)
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

            if (trip.UserId == userId)
                throw new BadRequestException("Trip.CreatorCannotJoin");

            if (trip.MaxParticipantsCount == trip.Participants.Count(x => x.Approved == true))
            {
                if (trip.Status != TripStatus.Ready)
                {
                    trip.Ready();
                    await unitOfWork.SaveChangesAsync();
                }
                throw new BadRequestException("Trip.MaxParticipantsReached");
            }

            TripParticipant? tripParticipant = trip.Participants
                .FirstOrDefault(x => x.UserId == userId);

            if (tripParticipant != null)
                return;

            tripParticipant = TripParticipant.Create(tripId, userId);
            repositoryFactory.Repository<TripParticipant>().Add(tripParticipant);
            if (trip.AutoApprove == true)
                tripParticipant.Approve();

            trip.Participants.Add(tripParticipant);
            repositoryFactory.Repository<Trip>().Update(trip);
            if (trip.MaxParticipantsCount == trip.Participants.Count(x => x.Approved == true))
                trip.Ready();

            await unitOfWork.SaveChangesAsync();
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
        public async Task UpdateUserJoinRequest(UpdateUserJoinRequestDto dto, Guid userId)
        {
            Trip trip = await repositoryFactory
                .Repository<Trip>()
                .GetFirstOrDefaultByFilter(
                    x => x.Id == dto.TripId && x.Status == TripStatus.Published && x.UserId == userId,
                    x => x.Include(x => x.Participants))
                ?? throw new NotFoundException("Trip.NotFound");

            if(trip.UserId == dto.UserId)
                throw new BadRequestException("Trip.CreatorCannotJoin");

            TripParticipant tripParticipant = trip.Participants
                .FirstOrDefault(x => x.UserId == dto.UserId)
                ?? throw new NotFoundException("TripParticipant.NotFound");

            if (dto.IsApproved == false)
            {
                tripParticipant.Reject();
                repositoryFactory.Repository<TripParticipant>().Delete(tripParticipant);
                await unitOfWork.SaveChangesAsync();
                return;
            }

            if (trip.MaxParticipantsCount == trip.Participants.Count(x => x.Approved == true))
            {
                await repositoryFactory.Repository<TripParticipant>().GetQueryable()
                    .Where(x => x.TripId == trip.Id && x.Approved == false)
                    .ExecuteDeleteAsync();
                if (trip.Status != TripStatus.Ready)
                    trip.Ready();
                await unitOfWork.SaveChangesAsync();
                throw new BadRequestException("Trip.MaxParticipantsReached");
            }

            if (tripParticipant.Approved == true)
                return;

            tripParticipant.Approve();
            repositoryFactory.Repository<TripParticipant>().Update(tripParticipant);
            await unitOfWork.SaveChangesAsync();
            if (trip.MaxParticipantsCount == trip.Participants.Count(x => x.Approved == true))
            {
                await repositoryFactory.Repository<TripParticipant>().GetQueryable()
                    .Where(x => x.TripId == trip.Id && x.Approved == false)
                    .ExecuteDeleteAsync();
                trip.Ready();
                await unitOfWork.SaveChangesAsync();
            } 
            return;
        }
        public async Task StartTrip(Guid tripId, Guid userId)
        {
            Trip trip = await repositoryFactory
                .Repository<Trip>()
                .GetFirstOrDefaultByFilter(
                    x => x.Id == tripId && x.Status == TripStatus.Ready && x.UserId == userId)
                ?? throw new NotFoundException("Trip.NotFound");
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
                ?? throw new NotFoundException("Trip.NotFound");
            trip.Complete();
            await unitOfWork.SaveChangesAsync();
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

        private async Task<Trip> CreateTripCore(
            CreateTripRequestDto dto,
            ICollection<string> roles,
            Guid userId,
            TripVisibility visibility,
            TripPublishMode publishMode,
            Guid? guideId,
            string notFoundKey,
            CancellationToken cancellationToken)
        {
            var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(u => u.Id == userId)
                ?? throw new NotFoundException(notFoundKey);

            var segments = await GetPlacesAsync(dto.Segments);
            var creatorRoles = roles.Select(x => Enum.Parse<UserRole>(x)).Aggregate(UserRole.User, (a, b) => a | b);

            var theme = await repositoryFactory.Repository<Theme>().GetFirstOrDefaultByFilter(t => t.Id == dto.ThemeId)
                ?? throw new NotFoundException("Theme.NotFound");

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
                visibility,
                publishMode,
                segments,
                dto.MaxParticipantsCount,
                guideId,
                dto.Segments.Select(s => s.Duration).ToList(),
                user,
                governorates,
                dto.StartDate);

            repositoryFactory.Repository<Trip>().Add(trip);

            return trip;
        }
        #endregion
    }
}
