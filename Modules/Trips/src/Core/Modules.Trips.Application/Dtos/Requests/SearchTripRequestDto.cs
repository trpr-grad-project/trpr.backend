using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Application.Dtos.Requests
{
    public class SearchTripRequestDto : BaseSearchTripRequestDto
    {
        public TripStatus? Status { get; set; }
    }
}
