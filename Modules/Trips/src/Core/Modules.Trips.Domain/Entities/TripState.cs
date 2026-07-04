using Modules.Trips.Domain.Abstractions;
using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Domain.Entities
{
    public partial class Trip
    {
        public string? RejectionReason { get; set; }
        public TripPublishMode PublishMode { get; set; } = TripPublishMode.DirectPublish;
        public TripStatus Status { get; set; } = TripStatus.UnderReview;

        public void SelectGuide(Guid guideId)
        {
            if (Status != TripStatus.Bidding)
                throw new InvalidOperationException("Only trips in Bidding status can have a guide selected.");
            GuideId = guideId;
            if (Participants.Count == MaxParticipantsCount)
                Status = TripStatus.Ready;
            else
                Status = TripStatus.Published;

        }
        public void Approve()
        {
            if (Status != TripStatus.UnderReview)
                throw new InvalidOperationException("Only trips under review can be approved.");
            if (PublishMode == TripPublishMode.DirectPublish)
                Status = TripStatus.Published;
            else
                Status = TripStatus.Bidding;
        }
        public void Reject(string reason)
        {
            if (Status != TripStatus.UnderReview)
                throw new InvalidOperationException("Only trips under review can be rejected.");
            Status = TripStatus.Rejected;
            RejectionReason = reason;
        }
        public void Complete()
        {
            if (Status != TripStatus.Bidding && Status != TripStatus.Started)
                throw new InvalidOperationException("Only Bidding or Started trips can be Finished.");
            Status = TripStatus.Finished;
        }
        public void Start()
        {
            if (Status != TripStatus.Published)
                throw new InvalidOperationException("Only published trips can be started.");
            Status = TripStatus.Started;
        }
        public void Cancel()
        {
            if (Status == TripStatus.Canceled)
                throw new InvalidOperationException("Trip is already canceled.");
            Status = TripStatus.Canceled;
        }
        public void Ready()
        {
            if (Status != TripStatus.Published)
                throw new InvalidOperationException("Only trips in Published status can be marked as Ready.");
            RaiseDomainEvent(new TripReadyDomainEvent(this.Id, this.Status));
            Status = TripStatus.Ready;
        }

    }
}
