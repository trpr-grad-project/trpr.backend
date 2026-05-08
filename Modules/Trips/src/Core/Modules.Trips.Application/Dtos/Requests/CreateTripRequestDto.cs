using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Application.Dtos.Requests
{
    public class CreateTripRequestDto
    {
        public string ThemeId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public ICollection<IFormFile> Images { get; set; } = [];
        public TripVisibility TripVisibility { get; set; }
        public ICollection<DayDto> Segments { get; set; } = [];
        public int MaxParticipantsCount { get; set; }
        public Guid? guideId { get; set; }
    }
}
