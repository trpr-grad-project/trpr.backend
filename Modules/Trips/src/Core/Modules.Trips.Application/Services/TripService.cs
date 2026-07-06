using Common.Application;
using Common.Application.Dtos;
using Common.Application.EventBus;
using Common.Application.Exceptions;
using Common.Domain.IntragationEvents;
using Microsoft.EntityFrameworkCore;
using Modules.Conversations.Contracts.Contracts;
using Modules.Conversations.Contracts.Dtos;
using Modules.Notifications.Contracts.Contracts;
using Modules.Notifications.Contracts.Dtos;
using Modules.Payments.Contracts.Contracts;
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
        IMapper<Trip, TripDetailsResponseDto> tripDetailsMapper,
        IConversationsContract conversationsContract,
        IPayContract payContract)
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
        public async Task<TripResponseDto> CreateTripByCompany(CompanyCreateTripRequestDto dto, Guid userId, CancellationToken cancellationToken)
        {
            var trip = await CreateTripCore(dto, UserRole.Company, userId, dto.Price,
                TripVisibility.Public, TripPublishMode.DirectPublish,
                guideId: dto.GuideId,
                notFoundKey: "Company.NotFound",
                cancellationToken);
            await UpdateStatus(new UpdateTripStatusRequestDto { Id = trip.Id, IsApproved = true }, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return tripMapper.Map(trip);
        }

        public async Task<TripResponseDto> CreateTripByGuide(GuideCreateTripRequestDto dto, Guid userId, CancellationToken cancellationToken)
        {
            // the creator is the guide
            var trip = await CreateTripCore(dto, UserRole.Guide, userId, dto.Price,
                TripVisibility.Public, TripPublishMode.DirectPublish,
                guideId: userId,
                notFoundKey: "Guide.NotFound",
                cancellationToken);

            return tripMapper.Map(trip);
        }
        public async Task<TripResponseDto> CreateTripByUser(UserCreateTripRequestDto dto, Guid userId, CancellationToken cancellationToken)
        {
            var trip = await CreateTripCore(dto, UserRole.User, userId, 0,
                dto.TripVisibility, dto.PublishMode,
                guideId: dto.GuideId,
                notFoundKey: "User.NotFound",
                cancellationToken);
            var tripParticipant = TripParticipant.Create(trip.Id, userId);
            tripParticipant.Approve();
            repositoryFactory.Repository<TripParticipant>().Add(tripParticipant);
            if(trip.TripVisibility == TripVisibility.Private)
                await UpdateStatus(new UpdateTripStatusRequestDto { Id = trip.Id, IsApproved = true }, cancellationToken);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return tripMapper.Map(trip);
        }
        public async Task<PaginationDto<TripResponseDto>> GetTrips(BaseSearchTripRequestDto request, Guid? userId = null, TripStatus? status = null, CancellationToken cancellationToken = default)
        {
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
                    x => x.Include(x => x.Participants).ThenInclude(x => x.User)
                    .Include(x => x.Guide))
                ?? throw new NotFoundException("Trip.NotFound");

            if (trip.UserId == userId)
                throw new BadRequestException("Trip.CreatorCannotJoin");

            if (trip.Status == TripStatus.Ready)
            {
                throw new BadRequestException("Trip.MaxParticipantsReached");
            }

            TripParticipant? tripParticipant = await repositoryFactory.Repository<TripParticipant>().GetFirstOrDefaultByFilter(
                x => x.TripId == tripId && x.UserId == userId, includes: x => x.Include(x => x.User)
            );

            if (tripParticipant != null)
                return;

            tripParticipant = TripParticipant.Create(tripId, userId);
            repositoryFactory.Repository<TripParticipant>().Add(tripParticipant);
            if (trip.AutoApprove == true)
            {
                await Pay(trip, trip.Price, tripParticipant.User);
                tripParticipant.Approve();
            }

            if (trip.MaxParticipantsCount == trip.Participants.Count(x => x.Approved == true))
            {
                trip.Ready();
                await conversationsContract.CreateConversation(new CreateConversationDto
                {
                    userId = userId,
                    Title = trip.Title,
                    ImageUrl = null,
                    ParticipantUserIds = trip.Participants.Select(x => x.UserId)
                        .Concat(trip.GuideId.HasValue ? new Guid[] { trip.GuideId.Value } : Array.Empty<Guid>())
                        .ToList()
                });
            }
            repositoryFactory.Repository<Trip>().Update(trip);

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

            if (trip.UserId == dto.UserId)
                throw new BadRequestException("Trip.CreatorCannotJoin");

            TripParticipant? tripParticipant = await repositoryFactory.Repository<TripParticipant>().GetFirstOrDefaultByFilter(
                x => x.TripId == trip.Id && x.UserId == dto.UserId, includes: x => x.Include(x => x.User)
                ) ?? throw new NotFoundException("TripParticipant.NotFound");

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
            await Pay(trip, trip.Price, tripParticipant.User);
            repositoryFactory.Repository<TripParticipant>().Update(tripParticipant);
            await unitOfWork.SaveChangesAsync();
            if (trip.MaxParticipantsCount == trip.Participants.Count(x => x.Approved == true))
            {
                await repositoryFactory.Repository<TripParticipant>().GetQueryable()
                    .Where(x => x.TripId == trip.Id && x.Approved == false)
                    .ExecuteDeleteAsync();
                trip.Ready();
                await conversationsContract.CreateConversation(new CreateConversationDto
                {
                    userId = userId,
                    Title = trip.Title,
                    ImageUrl = null,
                    ParticipantUserIds = trip.Participants.Select(x => x.UserId)
                        .Concat(trip.GuideId.HasValue ? new Guid[] { trip.GuideId.Value } : Array.Empty<Guid>())
                        .ToList()
                });
                await unitOfWork.SaveChangesAsync();
            }
            return;
        }
        public async Task StartTrip(Guid tripId, Guid userId)
        {
            Trip trip = await repositoryFactory
                .Repository<Trip>()
                .GetFirstOrDefaultByFilter(
                    x => x.Id == tripId && (x.Status == TripStatus.Published || x.Status == TripStatus.Ready) && x.UserId == userId)
                ?? throw new NotFoundException("Trip.NotFound");
            if(trip.Status == TripStatus.Published)
            {
                await conversationsContract.CreateConversation(new CreateConversationDto
                {
                    userId = userId,
                    Title = trip.Title,
                    ImageUrl = null,
                    ParticipantUserIds = trip.Participants.Select(x => x.UserId)
                        .Concat(trip.GuideId.HasValue ? new Guid[] { trip.GuideId.Value } : Array.Empty<Guid>())
                        .ToList()
                });
            }
            trip.Start();
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
        public async Task SubmitReviewAsync(
           Guid tripId,
           Guid reviewerId,
           ReviewTripRequestDto request,
           CancellationToken cancellationToken = default)
        {
            // Validate trip exists and is completed
            var trip = await repositoryFactory.Repository<Trip>()
                .GetFirstOrDefaultByFilter(
                    t => t.Id == tripId,
                    q => q.Include(t => t.Participants)
                        .Include(t => t.CreatedByUser)
                        .Include(t => t.Guide))
                ?? throw new NotFoundException("Trip.NotFound");

            if (trip.Status != TripStatus.Finished)
                throw new ConflictException("Trip.NotCompleted");

            // Verify reviewer is a participant
            var reviewerParticipant = trip.Participants.FirstOrDefault(p => p.UserId == reviewerId)
                ?? throw new ConflictException("Reviewer.NotParticipant");

            // Verify reviewee is not the reviewer
            if (request.RevieweeId == reviewerId)
                throw new ConflictException("Cannot.Review.Self");

            // Get the reviewee participant
            var revieweeParticipant = trip.Participants.FirstOrDefault(p => p.UserId == request.RevieweeId);

            // Determine if reviewee is in the trip (participant, guide, or creator)
            bool isValidReviewee = revieweeParticipant != null ||
                                    request.RevieweeId == trip.GuideId ||
                                    request.RevieweeId == trip.UserId;

            if (!isValidReviewee)
                throw new ConflictException("Reviewee.NotPartOfTrip");

            // Check if review already exists
            var existingReview = await repositoryFactory.Repository<TripReview>()
                .GetFirstOrDefaultByFilter(
                    r => r.TripId == tripId &&
                         r.ReviewerId == reviewerId &&
                         r.RevieweeId == request.RevieweeId);

            if (existingReview != null)
                throw new ConflictException("Review.Already.Exists");

            // Create review record
            var review = TripReview.Create(
                tripId,
                reviewerId,
                request.RevieweeId,
                request.Rating,
                request.Review);

            repositoryFactory.Repository<TripReview>().Add(review);

            // If reviewee is a participant, update their rating on trip
            if (revieweeParticipant != null)
            {
                revieweeParticipant.MakeReview(request.Rating, request.Review);
                repositoryFactory.Repository<TripParticipant>().Update(revieweeParticipant);
            }
            // If reviewee is the guide, update guide's user rating
            else if (request.RevieweeId == trip.GuideId)
            {
                if (request.Rating.HasValue)
                {
                    trip.Guide!.UpdateRating(request.Rating.Value);
                    repositoryFactory.Repository<User>().Update(trip.Guide);
                }
            }
            // If reviewee is the trip creator and creator is guide/company
            else if (request.RevieweeId == trip.UserId)
            {
                if (request.Rating.HasValue && (trip.CreatorRole == UserRole.Guide || trip.CreatorRole == UserRole.Company))
                {
                    trip.CreatedByUser.UpdateRating(request.Rating.Value);
                    repositoryFactory.Repository<User>().Update(trip.CreatedByUser);
                }
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<ICollection<ReviewResponseDto>> GetTripReviewsAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            var reviews = await repositoryFactory.Repository<TripReview>()
                .GetQueryable()
                .Where(r => r.TripId == tripId)
                .Include(r => r.Reviewer)
                .Include(r => r.Reviewee)
                .ToListAsync(cancellationToken);

            return reviews.Select(r => new ReviewResponseDto(
                r.ReviewerId,
                r.RevieweeId,
                r.ReviewerName,
                r.RevieweeName,
                r.Rating,
                r.Review
            )).ToList();
        }

        public async Task SubmitTripRatingAsync(
            Guid tripId,
            Guid reviewerId,
            SubmitTripRatingRequestDto request,
            CancellationToken cancellationToken = default)
        {
            // Validate trip exists and is completed
            var trip = await repositoryFactory.Repository<Trip>()
                .GetFirstOrDefaultByFilter(
                    t => t.Id == tripId,
                    q => q.Include(t => t.Participants)
                        .Include(t => t.CreatedByUser)
                        .Include(t => t.Guide))
                ?? throw new NotFoundException("Trip.NotFound");

            if (trip.Status != TripStatus.Finished)
                throw new ConflictException("Trip.NotCompleted");

            // Verify reviewer is a participant
            var reviewerParticipant = trip.Participants.FirstOrDefault(p => p.UserId == reviewerId)
                ?? throw new ConflictException("Reviewer.NotParticipant");

            // Check if trip rating already exists from this reviewer
            var existingRating = await repositoryFactory.Repository<TripRating>()
                .GetFirstOrDefaultByFilter(
                    r => r.TripId == tripId && r.ReviewerId == reviewerId);

            if (existingRating != null)
                throw new ConflictException("TripRating.Already.Exists");

            // Create trip rating record
            var tripRating = TripRating.Create(tripId, reviewerId, request.Rating, request.Review);
            repositoryFactory.Repository<TripRating>().Add(tripRating);

            // Update trip's aggregate rating if a rating was provided
            if (request.Rating.HasValue)
            {
                trip.UpdateRating(request.Rating.Value);
                repositoryFactory.Repository<Trip>().Update(trip);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<ICollection<TripRatingResponseDto>> GetTripRatingsAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            var ratings = await repositoryFactory.Repository<TripRating>()
                .GetQueryable()
                .Where(r => r.TripId == tripId)
                .Include(r => r.Reviewer)
                .ToListAsync(cancellationToken);

            return ratings.Select(r => new TripRatingResponseDto(
                r.ReviewerId,
                r.ReviewerName,
                r.Rating,
                r.Review
            )).ToList();
        }

        #region Helpers
        private async Task Pay(Trip trip, double price, User joiningUser) 
        {
            if(trip.CreatorRole.HasFlag(UserRole.Company))
            {
                await payContract.Pay(trip.Id.ToString(), joiningUser.Id, price, $"Paid {price} to Company {trip.CreatedByUser!.FirstName}");
                await payContract.Gain(trip.Id.ToString(),
                    trip.UserId, price,
                    $"Recieved {price} from User {joiningUser.FirstName + joiningUser.LastName} on Trip {trip.Id}");
            }
            else if (trip.GuideId != null)
            {
                await payContract.Pay(trip.Id.ToString(), joiningUser.Id, trip.Price, $"Paid {trip.Price} to Guide {trip.Guide!.FirstName + trip.Guide.LastName}");
                await payContract.Gain(trip.Id.ToString(),
                    trip.GuideId!.Value, trip.Price,
                    $"Recieved {trip.Price} from User {joiningUser.FirstName + joiningUser.LastName} on Trip {trip.Id}");
            }
        }
        private async Task<IDictionary<DateTime,ICollection<Place>>> GetPlacesAsync(ICollection<DayDto> source)
        {
            IDictionary<DateTime, ICollection<Place>> places = new Dictionary<DateTime, ICollection<Place>>();
            foreach (var day in source)
            {
                ICollection<Place> dayPlaces = await placeService.GetPlacesAsync(day.PlacesIds);
                places.Add(day.DayDate, dayPlaces);
            }
            return places;
        }

        public static IQueryable<Trip> FilterByBaseSearchRequest(IQueryable<Trip> query, BaseSearchTripRequestDto request)
        {
            if (request.TripType == TripType.ByGuides)
                query = query.Where(x => (x.CreatorRole & UserRole.Guide) == UserRole.Guide);
            else if (request.TripType == TripType.ByCompany)
                query = query.Where(x => (x.CreatorRole & UserRole.Company) == UserRole.Company);
            else if (request.TripType == TripType.Shared)
                query = query.Where(x => (x.CreatorRole & UserRole.User) == UserRole.User);


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
            UserRole role,
            Guid userId,
            double price,
            TripVisibility visibility,
            TripPublishMode publishMode,
            Guid? guideId,
            string notFoundKey,
            CancellationToken cancellationToken)
        {
            var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(u => u.Id == userId)
                ?? throw new NotFoundException(notFoundKey);

            var segments = await GetPlacesAsync(dto.Segments);

            var theme = await repositoryFactory.Repository<Theme>().GetFirstOrDefaultByFilter(t => t.Id == dto.ThemeId)
                ?? throw new NotFoundException("Theme.NotFound");
            var guide = guideId.HasValue ? await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(u => u.Id == guideId.Value) : null;
            var governorates = segments
                .SelectMany(x => x.Value)
                .Select(x => x.Governorate)
                .DistinctBy(x => x.Id)
                .ToList();
            var trip = Trip.Create(
                userId,
                theme,
                role,
                dto.Title,
                dto.AutoApprove,
                dto.Description,
                price,
                dto.Images,
                visibility,
                publishMode,
                segments,
                dto.MaxParticipantsCount,
                guideId,
                dto.Segments.Select(s => s.Duration).ToList(),
                governorates,
                dto.StartDate,
                dto.EndDate);

            repositoryFactory.Repository<Trip>().Add(trip);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return trip;
        }
        #endregion
    }
}
