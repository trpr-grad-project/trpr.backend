using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Entities
{
    public class Profile : Entity
    {
        public Guid Id { get; set; }
        public virtual User User { get; set; } = default!;
    }
}