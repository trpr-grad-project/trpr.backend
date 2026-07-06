using System;
using System.Collections.Generic;
using System.Text;
using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Trips.Application.Abstractions;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;
using Modules.Trips.Domain.ValueObjects;
using Modules.Users.Contracts.Contracts;
using Modules.Users.Contracts.Dtos;

namespace Modules.Trips.Application.Services
{
    public class ReviewService(RepositoryFactory repositoryFactory, IUnitOfWork unitOfWork, IUsersContract usersContract)
    {
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
                    q => q.Include(t => t.Participants).ThenInclude(p => p.User)
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

            // Get the reviewee participant (already loaded via trip.Participants, no extra query needed)
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
                var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(x => x.Id == revieweeParticipant.UserId)
                    ?? throw new NotFoundException("User.NotFound");
                if (request.Rating.HasValue)
                {
                    await ApplyRatingAndPropagate(user, request.Rating.Value, request.Review);
                }
            }
            // If reviewee is the guide, update guide's user rating
            else if (request.RevieweeId == trip.GuideId)
            {
                if (request.Rating.HasValue)
                {
                    await ApplyRatingAndPropagate(trip.Guide!, request.Rating.Value, request.Review);
                }
            }
            // If reviewee is the trip creator and creator is guide/company
            else if (request.RevieweeId == trip.UserId)
            {
                if (request.Rating.HasValue && (trip.CreatorRole == UserRole.Guide || trip.CreatorRole == UserRole.Company))
                {
                    await ApplyRatingAndPropagate(trip.CreatedByUser, request.Rating.Value, request.Review);
                }
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task ApplyRatingAndPropagate(User user, double newRating, string? review)
        {
            user.UpdateRating(newRating);
            repositoryFactory.Repository<User>().Update(user);

            await usersContract.UpdateUserRating(new UpdateUserRatingDto
            {
                UserId = user.Id,
                Rating = user.Rating!.Value,
                RatingCount = user.RatingCount!.Value,
                Review = review
            });
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
    }
}
