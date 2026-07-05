using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Trips.Domain.Abstractions;

namespace Modules.Trips.Domain.Entities
{
    public class Day : Entity
    {
        public Guid Id { get; set; }
        public int Order { get; set; } = 0;
        public double Duration { get; set; } = 0;
        public DateTime DayDate { get; set; }
        public Guid TripId { get; set; }
        public ICollection<Place> Places { get; set; } = [];
        public virtual Trip Trip { get; set; } = null!;

        public static Day Create(int order, double duration, DateTime dayTime, Guid tripId, ICollection<Place> places)
        {
            return new Day
            {
                Id = Guid.NewGuid(),
                Order = order,
                Duration = duration,
                TripId = tripId,
                Places = places
            };
        }
    }
}
