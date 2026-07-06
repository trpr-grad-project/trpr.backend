using System.Text.Json.Serialization;

namespace Modules.Trips.Infrastructure.Dtos.Responses;


public class TripPlanResponseDto
{
    [JsonPropertyName("active_trip")]
    public ActiveTripDto ActiveTrip { get; set; } = default!;
}

public class ActiveTripDto
{
    [JsonPropertyName("trip_id")]
    public Guid TripId { get; set; }

    [JsonPropertyName("itinerary")]
    public ItineraryDto Itinerary { get; set; } = default!;

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;
}

public class ItineraryDto
{

    [JsonPropertyName("daily_itinerary")]
    public List<DailyItineraryDto> DailyItinerary { get; set; } = [];

}

public class DailyItineraryDto
{
    [JsonPropertyName("day")]
    public int Day { get; set; }

    [JsonPropertyName("date")]
    public DateOnly Date { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("activities")]
    public List<ActivityDto> Activities { get; set; } = [];
}

public class ActivityDto
{
    [JsonPropertyName("place")]
    public TripPlaceDto Place { get; set; } = default!;

    [JsonPropertyName("time_range")]
    public string TimeRange { get; set; } = string.Empty;

    [JsonPropertyName("estimated_duration_hours")]
    public double EstimatedDurationHours { get; set; }
}

public class TripPlaceDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("desc")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("governorate")]
    public string Governorate { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public LocationDto Location { get; set; } = default!;

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = [];
}

public class LocationDto
{
    [JsonPropertyName("long")]
    public double Longitude { get; set; }

    [JsonPropertyName("late")]
    public double Latitude { get; set; }
}
