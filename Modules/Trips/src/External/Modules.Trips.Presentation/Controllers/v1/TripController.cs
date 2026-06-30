using Common.Application.Dtos;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
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
        public ICollection<string> UserRoles => User.GetRoles();
        [HttpGet("suggestion")]
        [Authorize]
        public async Task<ActionResult<object>> GetTripSuggestion([FromQuery] TripSuggestionRequestDto requestDto)
        {
            var result = await tripService.GetTripSuggestion(requestDto);
            return Ok(result);
        }

        [HttpGet("form-data")]
        [Authorize]
        public async Task<ActionResult<ThemeFormDataDto>> GetTripFormDate()
        {
            var result = await tripService.GetThemeFromData();
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TripResponseDto>> CreateTrip([FromBody] CreateTripRequestDto dto, CancellationToken cancellationToken)
        {
            var request = await tripService.CreateTrip(dto, UserRoles, UserId, cancellationToken);
            return Ok(request);
        }
        [HttpGet("home")]
        [Authorize]
        public async Task<ActionResult<HomeResponseDto>> GetHomePage([FromQuery] BaseSearchTripRequestDto request, CancellationToken cancellationToken)
        {
            var shared = await tripService.GetTrips(request.CloneWith(TripType.Shared), null, TripStatus.Published, cancellationToken);
            var byCompany = await tripService.GetTrips(request.CloneWith(TripType.ByCompany), null, TripStatus.Published, cancellationToken);
            var byGuide = await tripService.GetTrips(request.CloneWith(TripType.ByGuides), null, TripStatus.Published, cancellationToken);


            return Ok(new HomeResponseDto
            {
                Shared = shared,
                ByCompany = byCompany,
                ByGuide = byGuide
            });
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

        [HttpPut("join/{id:guid}")]
        [Authorize]
        public async Task<ActionResult> JoinTrip([FromRoute] Guid id)
        {
            await tripService.JoinTrip(id, UserId);
            return NoContent();
        }
        [HttpPut("accept/{id:guid}")]
        public async Task<ActionResult> AcceptJoin([FromRoute] Guid id)
        {
            await tripService.ApproveUserJoinRequest(id, UserId);
            return NoContent();
        }

        [HttpPut("start/{id:guid}")]
        [Authorize]
        public async Task<ActionResult> StartTrip([FromRoute] Guid id)
        {
            await tripService.StartTrip(id, UserId);
            return NoContent();
        }
        [HttpPut("end/{id:guid}")]
        [Authorize]
        public async Task<ActionResult> EndTrip([FromRoute] Guid id)
        {
            await tripService.EndTrip(id, UserId);
            return NoContent();
        }
    }
}
