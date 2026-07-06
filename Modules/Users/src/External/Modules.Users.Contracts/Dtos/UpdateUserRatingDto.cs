using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Users.Contracts.Dtos
{
    public class UpdateUserRatingDto
    {
        public Guid UserId { get; set; }
        public double? Rating { get; set; }
        public int? RatingCount { get; set; }
        public string? Review { get; set; }
    }
}
