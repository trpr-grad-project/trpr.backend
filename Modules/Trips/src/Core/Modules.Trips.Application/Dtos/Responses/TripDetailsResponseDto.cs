using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Application.Dtos.Responses
{
    public class TripDetailsResponseDto
    {
        public Guid Id { get; set; }
        public UserResponseDto CreatedByUser { get; set; } = default!;
        public List<string> CreatorRoles { get; set; } = default!;
        public string Theme { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public bool AutoApprove { get; set; }
        public DateOnly StartDate { get; set; }
        public string TripTime { get; set; } = string.Empty;
        public ICollection<string> ImagesUrls { get; set; } = [];
        public TripVisibility TripVisibility { get; set; }
        public TripStatus Status { get; set; }
        public TripPublishMode PublishMode { get; set; }
        public string? RejectionReason { get; set; }
        public List<DayResponseDto> Segments { get; set; } = [];
        public int MaxParticipantsCount { get; set; }
        public ICollection<UserResponseDto> ApprovedParticipants { get; set; } = [];
        public ICollection<UserResponseDto> PendingParticipants { get; set; } = [];
        public Guid? GuideId { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public BiddingCursorPageDto? BiddingsPage { get; set; } = null;
    }
}
