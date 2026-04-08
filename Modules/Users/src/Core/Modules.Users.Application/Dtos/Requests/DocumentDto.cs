using Microsoft.AspNetCore.Http;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Dtos.Requests
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public int GuideRequestId { get; set; }
        public DocumentType Type { get; set; }
        public IFormFile File { get; set; } = default!;
    }
}
