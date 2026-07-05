using Modules.Trips.Domain.Abstractions;
using Modules.Trips.Domain.ValueObjects;
using NetTopologySuite.Geometries;

namespace Modules.Trips.Domain.Entities
{
    public partial class Trip : Entity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User CreatedByUser { get; set; } = null!;
        // TODO replace themeID with theme nav property and update tripconfig
        // TODO add the role of the trip maker as a type (ENUM) and update the queries of all querying endpoints DONE
        public int ThemeId { get; set; }
        public bool AutoApprove { get; set; } = true;
        public Theme TripTheme { get; set; } = default!;
        public UserRole CreatorRole { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double ActualDuration { get; set; }
        public double ExpectedDuration { get; set; }
        public ICollection<string> Images { get; set; } = [];
        public Point Centroid { get; set; } = default!;
        public TripVisibility TripVisibility { get; set; } = TripVisibility.Public;
        public virtual ICollection<Day> Segments { get; set; } = [];
        public virtual ICollection<TripGovernorate> TripGovernorates { get; set; } = [];
        public virtual ICollection<TripBidding> Bids { get; set; } = [];
        public int MaxParticipantsCount { get; set; }
        public Guid? GuideId { get; set; }
        public ICollection<TripParticipant> Participants { get; set; } = [];
        public static Trip Create(
            Guid userId,
            Theme theme,
            UserRole roles,
            string title,
            bool autoApprove,
            string description,
            double price,
            ICollection<string> images,
            TripVisibility tripVisibility,
            TripPublishMode publishMode,
            IDictionary<DateTime,ICollection<Place>> segments,
            int maxParticipantCount,
            Guid? guideId,
            List<double> duration,
            User user,
            ICollection<Governorate> governorates,
            DateTime startDate,
            DateTime endDate)
        {
            Trip newTrip = new()
            {
                PublishMode = publishMode,
                Id = Guid.NewGuid(),
                UserId = userId,
                ThemeId = theme.Id,
                TripTheme = theme,
                Title = title,
                CreatorRole = roles,
                Description = description,
                AutoApprove = autoApprove,
                Price = price,
                Images = images,
                TripVisibility = tripVisibility,
                MaxParticipantsCount = maxParticipantCount,
                GuideId = guideId,
                CreatedByUser = user,
                Status = TripStatus.UnderReview,
                StartDate = startDate,
                EndDate = endDate,
            };
            foreach (double dur in duration)
            {
                newTrip.ExpectedDuration += dur;
            }
            ICollection<Day> days = [];
            int order = 1;
            foreach (var segment in segments)
            {
                Day day = Day.Create(order, duration[order - 1], segment.Key, newTrip.Id, segment.Value);
                days.Add(day);
            }
            newTrip.Segments = days;
            MultiPoint points = new(
                [.. segments
                    .SelectMany(s => s.Value)
                    .Select(p => p.Location)]
                )
            { SRID = 4326 };
            newTrip.Centroid = points.Centroid;
            ICollection<TripGovernorate> tripGovernorates = [];
            foreach (var governorate in governorates)
            {
                TripGovernorate tripGovernorate = new TripGovernorate
                {
                    TripId = newTrip.Id,
                    GovernorateId = governorate.Id
                };
                tripGovernorates.Add(tripGovernorate);
            }
            newTrip.TripGovernorates = tripGovernorates;
            return newTrip;
        }

        // update method for adding/removing participants
    }
}
