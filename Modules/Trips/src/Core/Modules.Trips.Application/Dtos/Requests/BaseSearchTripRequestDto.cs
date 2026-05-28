using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Application.Dtos.Requests
{
    public class BaseSearchTripRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public int? ThemeId { get; set; }
        public int? GovernorateId { get; set; }
        public string? Title { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? RadiusInMeters { get; set; }
    }
}
