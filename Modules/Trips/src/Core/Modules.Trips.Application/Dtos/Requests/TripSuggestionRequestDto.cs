namespace Modules.Trips.Application.Dtos.Requests;

public class TripSuggestionRequestDto
{
    public int ThemeId { get; set; }
    public int NumberOfDays { get; set; }
    public DateTime StartDateUtc { get; set; } = DateTime.UtcNow;
    public int? GovernorateId { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public int? RadiusInMeters { get; set; }
}
