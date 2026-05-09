namespace Modules.Trips.Application.Dtos.Requests
{
    public class SearchTripRequestDto
    {
        public int? ThemeId { get; set; }
        public int? GovernorateId { get; set; }
        public string? Title { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
    }
}
