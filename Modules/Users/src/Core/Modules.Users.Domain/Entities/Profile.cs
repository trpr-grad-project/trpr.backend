using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Entities
{
    public class Language : Entity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string NativeName { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
    public class Interest : Entity
    {
        public Guid Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
    public class Vibe : Entity
    {
        public Guid Id { get; set; }
        public string Thumbnail { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
    public class ProfileLanguage
    {
        public Guid ProfileId { get; set; }
        public Guid LanguageId { get; set; }
        public virtual Profile Profile { get; set; } = default!;
        public virtual Language Language { get; set; } = default!;
    }

    public class ProfileInterest
    {
        public Guid ProfileId { get; set; }
        public Guid InterestId { get; set; }
        public virtual Profile Profile { get; set; } = default!;
        public virtual Interest Interest { get; set; } = default!;
    }

    public class ProfileVibe
    {
        public Guid ProfileId { get; set; }
        public Guid VibeId { get; set; }
        public virtual Profile Profile { get; set; } = default!;
        public virtual Vibe Vibe { get; set; } = default!;
    }

    public class Profile : Entity
    {
        public Guid Id { get; set; }
        public virtual User User { get; set; } = default!;
        
        // Navigation properties
        public virtual ICollection<ProfileLanguage> Languages { get; set; } = new List<ProfileLanguage>();
        public virtual ICollection<ProfileInterest> Interests { get; set; } = new List<ProfileInterest>();
        public virtual ICollection<ProfileVibe> Vibes { get; set; } = new List<ProfileVibe>();
    }
}