using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Services;

namespace Modules.Trips.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/bidding")]
    public class BiddingController(BiddingService biddingService) : ControllerBase
    {
        public Guid UserId => User.GetUserId();

        [HttpPost]
        [Authorize(Roles = "Guide")]
        public async Task<ActionResult> PlaceBid([FromBody] CreateBiddingRequestDto dto, CancellationToken cancellationToken)
        {
            await biddingService.PlaceBidAsync(dto, UserId, cancellationToken);
            return NoContent();
        }

        [HttpGet("{tripId:guid}")]
        [Authorize]
        public async Task<ActionResult<BiddingCursorPageDto>> GetTripBiddings(
            Guid tripId,
            [FromQuery] GetTripBiddingsQueryDto query,
            CancellationToken cancellationToken)
        {
            var result = await biddingService.GetTripBiddingsAsync(tripId, UserId, query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("{tripId:guid}/select/{biddingId:guid}")]
        [Authorize]
        public async Task<ActionResult> SelectBidding(Guid tripId, Guid biddingId, CancellationToken cancellationToken)
        {
            await biddingService.SelectBidAsync(tripId, biddingId, UserId, cancellationToken);
            return NoContent();
        }
    }
}
