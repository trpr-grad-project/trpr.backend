using Modules.Trips.Domain.Abstractions;
using Modules.Trips.Domain.Events;
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
            RaiseDomainEvent(
                new TripStatusUpdatedDomainEVent(this.Id, TripStatus.Bidding)
            );
        }
        public void Approve()
        {
            if (Status != TripStatus.UnderReview)
                throw new InvalidOperationException("Only trips under review can be approved.");
            if (PublishMode == TripPublishMode.DirectPublish)
                Status = TripStatus.Published;
            else
                Status = TripStatus.Bidding;
            RaiseDomainEvent(
                new TripStatusUpdatedDomainEVent(this.Id, TripStatus.UnderReview)
            );
        }
        public void Reject(string reason)
        {
            if (Status != TripStatus.UnderReview)
                throw new InvalidOperationException("Only trips under review can be rejected.");
            Status = TripStatus.Rejected;
            RejectionReason = reason;
            RaiseDomainEvent(
                new TripStatusUpdatedDomainEVent(this.Id, TripStatus.UnderReview)
            );
        }
        public void Complete()
        {
            if (Status != TripStatus.Started)
                throw new InvalidOperationException("Only Bidding or Started trips can be Finished.");
            Status = TripStatus.Finished;
            RaiseDomainEvent(
                new TripStatusUpdatedDomainEVent(this.Id, TripStatus.Started)
            );
        }
        public void Start()
        {
            if (Status != TripStatus.Published)
                throw new InvalidOperationException("Only published trips can be started.");
            Status = TripStatus.Started;
            RaiseDomainEvent(
               new TripStatusUpdatedDomainEVent(this.Id, TripStatus.Published)
           );
        }
        public void Cancel()
        {
            if (Status == TripStatus.Canceled)
                throw new InvalidOperationException("Trip is already canceled.");
            Status = TripStatus.Canceled;
            RaiseDomainEvent(
                new TripStatusUpdatedDomainEVent(this.Id, TripStatus.Canceled)
            );
        }
        public void Ready()
        {
            if (Status != TripStatus.Published)
                throw new InvalidOperationException("Only trips in Published status can be marked as Ready.");
            Status = TripStatus.Ready;
            RaiseDomainEvent(
              new TripStatusUpdatedDomainEVent(this.Id, TripStatus.Published)
            );
        }

    }
}
