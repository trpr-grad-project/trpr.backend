using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Users.Application.Dtos.Requests
{
    public class CreateCommentRequestDto
    {
        public Guid PostId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ParentCommentId { get; set; } = null;
    }
}