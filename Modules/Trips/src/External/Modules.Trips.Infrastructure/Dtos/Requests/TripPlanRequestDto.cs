using System.Text.Json.Serialization;

namespace Modules.Trips.Infrastructure.Dtos.Requests;

public class TripPlanRequestDto
{
    [JsonPropertyName("theme")]
    public string Theme { get; set; } = string.Empty;

    [JsonPropertyName("places")]
    public List<TripPlanPlaceDto> Places { get; set; } = new();

    [JsonPropertyName("numberOfDays")]
    public int NumberOfDays { get; set; }

    [JsonPropertyName("thread_id")]
    public string ThreadId { get; set; } = "default";

    [JsonPropertyName("destination_city")]
    public string DestinationCity { get; set; } = string.Empty;

    [JsonPropertyName("date_from")]
    public DateTime DateFrom { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("date_to")]
    public DateTime DateTo => DateFrom.AddDays(NumberOfDays);
}

public class TripPlanPlaceDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("desc")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("averageVisitTime")]
    public double AverageVisitTime { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("governorate")]
    public string Governorate { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public TripPlanLocationDto Location { get; set; } = new();

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = new();
}

public class TripPlanLocationDto
{
    [JsonPropertyName("long")]
    public double Longitude { get; set; }

    [JsonPropertyName("late")]
    public double Latitude { get; set; }
}
