using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Dtos.Responses
{
    public class GuideUpgradeResponseDto
    {
        public Guid Id { get; set; }
        public ApproveStatus Status { get; set; } = ApproveStatus.Pending;
        public string? RejectionReason { get; set; }
        public int? adminId { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<DocumentDto> Documents { get; set; } = [];
        public UserResponseDto user { get; set; } = null!;
    }
}
