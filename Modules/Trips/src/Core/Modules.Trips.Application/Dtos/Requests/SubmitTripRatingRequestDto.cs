namespace Modules.Trips.Application.Dtos.Requests
{
    public record SubmitTripRatingRequestDto(Guid TripId, double? Rating, string? Review);
}
