using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Trips.Application.Dtos.Responses
{
    public class HomeResponseDto
    {
        public List<TripResponseDto>? Shared { get; set; }
        public List<TripResponseDto>? ByCompany { get; set; }
        public List<TripResponseDto>? ByGuide { get; set; }
    }
}
