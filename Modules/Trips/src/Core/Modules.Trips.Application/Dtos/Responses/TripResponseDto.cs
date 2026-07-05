using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Application.Dtos.Responses
{
    public class TripResponseDto
    {
        public Guid TripId { get; set; }
        public Guid CreatedByUser { get; set; }
        public string Theme { get; set; } = string.Empty;
        public List<string> CreatorRoles { get; set; } = default!;
        public string Title { get; set; } = string.Empty;
        public bool AutoApprove { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<string> ImagesUrls { get; set; } = [];
        public TripVisibility TripVisibility { get; set; }
        public TripStatus Status { get; set; }
        public List<DayResponseDto> Segments { get; set; } = [];
        public string TripTime { get; set; } = string.Empty;
        public int MaxParticipantsCount { get; set; }
        public Guid? GuideId { get; set; }
        public string? RejectionReason { get; set; }
    }
}
