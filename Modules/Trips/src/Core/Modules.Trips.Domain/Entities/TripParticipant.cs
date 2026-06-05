using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Trips.Domain.Abstractions;

namespace Modules.Trips.Domain.Entities
{
    public class TripParticipant : Entity
    {
        // TODO migrate and make endpoints for reviews and ratings
        // TODO make join endpoint
        public Guid TripId { get; set; }
        public Guid UserId { get; set; }
        public virtual Trip Trip { get; set; } = default!;
        public virtual User User { get; set; } = default!;
        public double? TripRating { get; set; }
        public double? TripOwnerRating { get; set; }
        public string? GuideReview { get; set; }
        public string? TripReview { get; set; }
    }
}
