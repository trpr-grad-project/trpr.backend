using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace Modules.Trips.Domain.Entities
{
    public class Place
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double? Rating { get; set; } = null;
        public double? AverageVisitTime { get; set; } = null;
        public string? OsrmId { get; set; } = null;
        public int VisitCount { get; set; } = 0;
        public int RateCount { get; set; } = 0;
        public int CategoryId { get; set; }
        public int GovernorateId { get; set; }
        public Point Location { get; set; } = default!;
        public Governorate Governorate { get; set; } = default!;
        public Category Category { get; set; } = default!;
    }
}