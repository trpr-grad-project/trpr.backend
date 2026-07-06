using System;
using System.Collections.Generic;
using System.Text;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Services;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/review")]
    public class ReviewController(ReviewService reviewService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();

        [HttpPost("{tripId:guid}/review")]
        [Authorize]
        public async Task<ActionResult> SubmitReview(
            [FromRoute] Guid tripId,
            [FromBody] ReviewTripRequestDto request,
            CancellationToken cancellationToken)
        {
            await reviewService.SubmitReviewAsync(tripId, UserId, request, cancellationToken);
            return NoContent();
        }

        [HttpGet("{tripId:guid}/reviews")]
        [Authorize]
        public async Task<ActionResult<ICollection<ReviewResponseDto>>> GetTripReviews(
            [FromRoute] Guid tripId,
            CancellationToken cancellationToken)
        {
            var reviews = await reviewService.GetTripReviewsAsync(tripId, cancellationToken);
            return Ok(reviews);
        }

        [HttpPost("{tripId:guid}/rating")]
        [Authorize]
        public async Task<ActionResult> SubmitTripRating(
            [FromRoute] Guid tripId,
            [FromBody] SubmitTripRatingRequestDto request,
            CancellationToken cancellationToken)
        {
            await reviewService.SubmitTripRatingAsync(tripId, UserId, request, cancellationToken);
            return NoContent();
        }

        [HttpGet("{tripId:guid}/ratings")]
        [Authorize]
        public async Task<ActionResult<ICollection<TripRatingResponseDto>>> GetTripRatings(
            [FromRoute] Guid tripId,
            CancellationToken cancellationToken)
        {
            var ratings = await reviewService.GetTripRatingsAsync(tripId, cancellationToken);
            return Ok(ratings);
        }
    }
}