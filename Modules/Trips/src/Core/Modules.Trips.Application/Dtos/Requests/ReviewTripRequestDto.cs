namespace Modules.Trips.Application.Dtos.Requests;

public record ReviewTripRequestDto(
    Guid TripId,
    Guid RevieweeId,
    double? Rating,
    string? Review
);
