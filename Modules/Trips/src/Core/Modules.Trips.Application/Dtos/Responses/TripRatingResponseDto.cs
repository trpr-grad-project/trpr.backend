namespace Modules.Trips.Application.Dtos.Responses
{
    public record TripRatingResponseDto(Guid ReviewerId, string ReviewerName, double? Rating, string? Review);
}
