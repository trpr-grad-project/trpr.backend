using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Trips.Domain.Abstractions;
using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Domain.Entities
{
    public class Trip : Entity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User CreatedByUser { get; set; } = null!;
        public string ThemeId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public double ActualDuration { get; set; }
        public double ExpectedDuration { get; set; }
        public ICollection<string> Images { get; set; } = [];
        public TripVisibility TripVisibility { get; set; } = TripVisibility.Public;
        public virtual ICollection<Day> Segments { get; set; } = [];
        public int MaxParticipantsCount { get; set; }
        public Guid? GuideId { get; set; }
        public ICollection<TripParticipant> Participants { get; set; } = [];
        public static Trip Create(Guid userId, string themeId, string title, string description, 
            double price, ICollection<string> images, 
            TripVisibility tripVisibility, ICollection<ICollection<Place>> segments, 
            int maxParticipantCount, Guid? guideId, List<double> duration, User user)
        {
            Trip newTrip = new Trip();
            newTrip.Id = Guid.NewGuid();
            newTrip.UserId = userId;
            newTrip.ThemeId = themeId;
            newTrip.Title = title;
            newTrip.Description = description;
            newTrip.Price = price;
            newTrip.Images = images;
            newTrip.TripVisibility = tripVisibility;
            newTrip.MaxParticipantsCount = maxParticipantCount;
            newTrip.GuideId = guideId;
            newTrip.CreatedByUser = user;
            foreach (double dur in duration)
            {
                newTrip.ExpectedDuration += dur;
            }
            ICollection<Day> days = [];
            foreach(var segment in segments)
            {
                Day day = new Day
                {
                    Id = Guid.NewGuid(),
                    TripId = newTrip.Id,
                    Places = segment
                };
                days.Add(day);
            }
            newTrip.Segments = days;
            return newTrip;
        }
        // update method for adding/removing participants
        // update method for adding new places?
        // update method for trip details
    }
}
