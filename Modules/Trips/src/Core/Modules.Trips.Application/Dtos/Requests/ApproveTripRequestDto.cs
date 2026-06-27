namespace Modules.Trips.Presentation.Controllers.v1
{
    public class ApproveTripRequestDto
    {
        public Guid TripId { get; set; }
        public Guid UserId { get; set; }
    }
}