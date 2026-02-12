using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Domain.Entities
{
    public class GuideUpgradeRequest
    {
        public Guid Id { get; set; }
        public int userId { get; set; }
        public ApproveStatus Status { get; set; } = ApproveStatus.Rejected;
        public string? RejectionReason { get; set; }
        public int? adminId { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        // nav property
        public User user { get; set; } = null!;
    }
}