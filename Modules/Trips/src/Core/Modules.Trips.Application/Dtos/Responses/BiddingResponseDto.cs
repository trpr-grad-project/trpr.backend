namespace Modules.Trips.Application.Dtos.Responses
{
    public class BiddingResponseDto
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public Guid GuideId { get; set; }
        public string GuideUsername { get; set; } = string.Empty;
        public string GuideFirstName { get; set; } = string.Empty;
        public string GuideLastName { get; set; } = string.Empty;
        public double ProposedPrice { get; set; }
        public string? ProposalMessage { get; set; }
        public DateTime CreatedAtUTC { get; set; }
    }
}
