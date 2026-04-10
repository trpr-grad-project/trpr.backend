using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Domain.Entities
{
    public class GuideUpgradeRequest
    {
        public Guid Id { get; set; }
        public Guid userId { get; set; }
        public ApproveStatus Status { get; set; } = ApproveStatus.Rejected;
        public string? RejectionReason { get; set; }
        public int? adminId { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual ICollection<Document> Documents { get; set; } = [];
        public virtual User user { get; set; } = null!;
        public static GuideUpgradeRequest Create(Guid userid, ICollection<Document> documents)
        {
            var upgradeRequest = new GuideUpgradeRequest
            {
                Id = Guid.NewGuid(),
                userId = userid,
                CreatedAt = DateTime.UtcNow,
                Status = ApproveStatus.Pending,
            };
            ICollection<Document> docs = [];
            foreach (var document in documents)
            {
                var docm = new Document
                {
                    Id = Guid.NewGuid(),
                    GuideRequestId = upgradeRequest.Id,
                    Type = document.Type,
                    UploadedAt = upgradeRequest.CreatedAt,
                    FileUrl = document.FileUrl,
                };
                docs.Add(docm);
            }
            upgradeRequest.Documents = docs;
            return upgradeRequest;
        }
        //update
    }
}