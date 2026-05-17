using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Services;

namespace Modules.Trips.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/trip")]
    public class TripController(TripService tripService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();
        [HttpPost("create-trip")]
        [Authorize]
        public async Task<ActionResult<TripResponseDto>> CreateTrip([FromBody] CreateTripRequestDto dto, CancellationToken cancellationToken)
        {
            var request = await tripService.CreateTrip(dto, UserId, cancellationToken);
            return Ok(request);
        }

        [HttpGet("get-trips")]
        public async Task<ActionResult<TripResponseDto>> GetTrips([FromQuery] SearchTripRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = await tripService.GetTrips(requestDto, cancellationToken);
            return Ok(request);
        }
    }
}
