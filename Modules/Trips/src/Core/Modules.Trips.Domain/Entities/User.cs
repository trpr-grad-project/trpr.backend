using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Modules.Trips.Domain.Abstractions;

namespace Modules.Trips.Domain.Entities
{
    public class User : Entity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public double? Rating { get; set; } = null;
        public int? RatingCount { get; set; } = null;
        public virtual ICollection<Trip> CreatedTrips { get; set; } = [];
        public virtual ICollection<TripBidding> Bids { get; set; } = [];
        public virtual ICollection<TripParticipant> JoinedTrips { get; set; } = [];

        public static User Create(Guid Id, string UserName, string FirstName, string LastName)
        {
            return new User
            {
                Id = Id,
                UserName = UserName,
                FirstName = FirstName,
                LastName = LastName,
                Email = MailAddress.TryCreate(UserName, out var _) ? UserName : null,
                PhoneNumber = MailAddress.TryCreate(UserName, out var _) ? null : UserName,
            };
        }
        public void UpdateRating(double newRating)
        {
            if (Rating.HasValue && RatingCount.HasValue)
            {
                double totalRating = Rating.Value * RatingCount.Value;
                totalRating += newRating;
                RatingCount++;
                Rating = totalRating / RatingCount.Value;
            }
            else
            {
                Rating = newRating;
                RatingCount = 1;
            }
        }
    }
}
