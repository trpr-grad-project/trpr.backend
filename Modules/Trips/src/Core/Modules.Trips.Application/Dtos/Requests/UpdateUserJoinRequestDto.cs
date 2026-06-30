using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Trips.Application.Dtos.Requests
{
    public class UpdateUserJoinRequestDto
    {
        public bool IsApproved { get; set; }
        public Guid TripId { get; set; }
        public Guid UserId { get; set; }
    }
}
