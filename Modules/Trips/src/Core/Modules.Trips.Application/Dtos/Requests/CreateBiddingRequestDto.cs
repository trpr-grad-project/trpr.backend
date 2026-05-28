namespace Modules.Trips.Application.Dtos.Requests
{
    public class CreateBiddingRequestDto
    {
        public Guid TripId { get; set; }
        public double ProposedPrice { get; set; }
        public string? ProposalMessage { get; set; }
    }
}
