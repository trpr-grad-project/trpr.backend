using Common.Application.Dtos;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Services;
using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/trip")]
    public class TripController(TripService tripService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TripResponseDto>> CreateTrip([FromBody] CreateTripRequestDto dto, CancellationToken cancellationToken)
        {
            var request = await tripService.CreateTrip(dto, UserId, cancellationToken);
            return Ok(request);
        }

        [HttpPost("change-status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ApproveTrip(UpdateTripStatusRequestDto dto, CancellationToken cancellationToken)
        {
            await tripService.UpdateStatus(dto, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<TripDetailsResponseDto>> GetTripDetails(Guid id, CancellationToken cancellationToken)
        {
            var result = await tripService.GetTripDetailsAsync(id, UserId, cancellationToken);
            return Ok(result);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaginationDto<TripResponseDto>>> GetTrips([FromQuery] SearchTripRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = await tripService.GetTrips(requestDto, null, null, cancellationToken);
            return Ok(request);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<PaginationDto<TripResponseDto>>> GetMyTrips([FromQuery] SearchTripRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = await tripService.GetTrips(requestDto, UserId, null, cancellationToken);
            return Ok(request);
        }


        [HttpGet("bidding")]
        [Authorize(Roles = "Admin,Guide")]
        public async Task<ActionResult<PaginationDto<TripResponseDto>>> GetTripsForBidding([FromQuery] BaseSearchTripRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = await tripService.GetTrips(requestDto, null, TripStatus.Bidding, cancellationToken);
            return Ok(request);
        }

        [HttpGet("published")]
        [Authorize]
        public async Task<ActionResult<PaginationDto<TripResponseDto>>> GetAllTrips([FromQuery] BaseSearchTripRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = await tripService.GetTrips(requestDto, null, TripStatus.Published, cancellationToken);
            return Ok(request);
        }
    }
}
