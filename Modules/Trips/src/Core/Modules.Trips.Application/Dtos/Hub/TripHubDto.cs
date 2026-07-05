using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Trips.Application.Dtos.Hub
{
    public class TripHubDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public string? Description { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public double? ActualDuration { get; init; }
        public double ExpectedDuration { get; init; }
        public double Price { get; init; }
        public ICollection<string> Images { get; init; } = [];
        public int MaxParticipantsCount { get; init; }
        public Guid? GuideId { get; init; }
        public string Status { get; init; } = default!;
    }
}