using System.Text.Json.Serialization;

namespace Modules.Trips.Infrastructure.Dtos.Responses;

public class TripPlanResponseDto
{
    [JsonPropertyName("active_trip")]
    public ActiveTripDto? ActiveTrip { get; set; }

    [JsonPropertyName("upcoming_events")]
    public List<UpcomingEventDto> UpcomingEvents { get; set; } = new();
}

public class ActiveTripDto
{
    [JsonPropertyName("trip_id")]
    public string TripId { get; set; } = string.Empty;

    [JsonPropertyName("itinerary")]
    public ItineraryDto? Itinerary { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;
}

public class ItineraryDto
{
    [JsonPropertyName("trip_info")]
    public TripInfoDto? TripInfo { get; set; }

    [JsonPropertyName("city_introduction")]
    public List<string> CityIntroduction { get; set; } = new();

    [JsonPropertyName("daily_itinerary")]
    public List<DailyItineraryDto> DailyItinerary { get; set; } = new();

    [JsonPropertyName("suggested_places")]
    public SuggestedPlacesDto? SuggestedPlaces { get; set; }

    [JsonPropertyName("budget")]
    public BudgetDto? Budget { get; set; }

    [JsonPropertyName("travel_tips")]
    public List<TravelTipDto> TravelTips { get; set; } = new();

    [JsonPropertyName("itinerary_adjustments")]
    public List<ItineraryAdjustmentDto> ItineraryAdjustments { get; set; } = new();
}

public class TripInfoDto
{
    [JsonPropertyName("destination_city")]
    public string DestinationCity { get; set; } = string.Empty;

    [JsonPropertyName("start_date")]
    public string StartDate { get; set; } = string.Empty;

    [JsonPropertyName("end_date")]
    public string EndDate { get; set; } = string.Empty;

    [JsonPropertyName("traveler_interests")]
    public List<string> TravelerInterests { get; set; } = new();
}

public class DailyItineraryDto
{
    [JsonPropertyName("day")]
    public int Day { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("activities")]
    public List<ActivityDto> Activities { get; set; } = new();
}

public class ActivityDto
{
    [JsonPropertyName("time_range")]
    public string TimeRange { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;

    [JsonPropertyName("estimated_duration_hours")]
    public int EstimatedDurationHours { get; set; }
}

public class SuggestedPlacesDto
{
    [JsonPropertyName("cafes")]
    public List<CafeDto> Cafes { get; set; } = new();

    [JsonPropertyName("restaurants")]
    public List<RestaurantDto> Restaurants { get; set; } = new();

    [JsonPropertyName("attractions")]
    public List<AttractionDto> Attractions { get; set; } = new();

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("rating")]
    public int Rating { get; set; }
}

public class CafeDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("price_range")]
    public string PriceRange { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;
}

public class RestaurantDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("cuisine_type")]
    public string CuisineType { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("rating")]
    public int Rating { get; set; }
}

public class AttractionDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

public class BudgetDto
{
    [JsonPropertyName("accommodation_per_night")]
    public string AccommodationPerNight { get; set; } = string.Empty;

    [JsonPropertyName("food_per_meal")]
    public string FoodPerMeal { get; set; } = string.Empty;

    [JsonPropertyName("cafe_drinks")]
    public string CafeDrinks { get; set; } = string.Empty;

    [JsonPropertyName("transport_per_day")]
    public string TransportPerDay { get; set; } = string.Empty;

    [JsonPropertyName("attractions")]
    public string Attractions { get; set; } = string.Empty;

    [JsonPropertyName("events")]
    public string Events { get; set; } = string.Empty;

    [JsonPropertyName("total_estimated_cost")]
    public string TotalEstimatedCost { get; set; } = string.Empty;
}

public class TravelTipDto
{
    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("advice")]
    public string Advice { get; set; } = string.Empty;
}

public class ItineraryAdjustmentDto
{
    [JsonPropertyName("scenario")]
    public string Scenario { get; set; } = string.Empty;

    [JsonPropertyName("suggestion")]
    public string Suggestion { get; set; } = string.Empty;
}

public class UpcomingEventDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;
}
