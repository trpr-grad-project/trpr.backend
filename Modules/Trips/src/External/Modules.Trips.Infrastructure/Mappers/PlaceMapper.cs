using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Domain.Entities;
using Modules.Trips.Infrastructure.Dtos.Requests;
namespace Modules.Trips.Infrastructure.Mappers;

public static class PlaceMapper
{
    public static TripPlanPlaceDto ToTripPlanPlaceDto(this PlaceDto place)
    {
        return new TripPlanPlaceDto
        {
            Id = place.Id,
            Title = place.Title,
            Description = place.Description,
            AverageVisitTime = place.AverageVisitTime ?? 1.0,
            Category = place.Category.Name,
            Governorate = place.Governorate.Name,
            Location = new TripPlanLocationDto
            {
                Longitude = place.Longitude,
                Latitude = place.Latitude
            },
            Tags = place.Tags.Select(t => t.Name).ToList()
        };
    }
}
