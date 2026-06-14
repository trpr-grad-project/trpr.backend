using System;
using System.Collections.Generic;
using System.Text;
using Common.Application.Dtos;

namespace Modules.Trips.Application.Dtos.Responses
{
    public class HomeResponseDto
    {
        public PaginationDto<TripResponseDto>? Shared { get; set; }
        public PaginationDto<TripResponseDto>? ByCompany { get; set; }
        public PaginationDto<TripResponseDto>? ByGuide { get; set; }
    }
}
