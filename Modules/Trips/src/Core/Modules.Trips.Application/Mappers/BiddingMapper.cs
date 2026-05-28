using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Mappers
{
    public static class BiddingMapper
    {
        public static BiddingResponseDto ToBiddingResponseDto(this TripBidding source)
        {
            return new BiddingResponseDto
            {
                Id = source.Id,
                TripId = source.TripId,
                GuideId = source.GuideId,
                GuideUsername = source.Guide.UserName,
                GuideFirstName = source.Guide.FirstName,
                GuideLastName = source.Guide.LastName,
                ProposedPrice = source.ProposedPrice,
                ProposalMessage = source.ProposalMessage,
                CreatedAtUTC = source.CreatedAtUTC
            };
        }
    }
}
