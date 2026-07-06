namespace Modules.Trips.Application.Dtos.Responses;

public record ReviewResponseDto(
    Guid ReviewerId,
    Guid RevieweeId,
    string ReviewerName,
    string RevieweeName,
    double? Rating,
    string? Review
);
