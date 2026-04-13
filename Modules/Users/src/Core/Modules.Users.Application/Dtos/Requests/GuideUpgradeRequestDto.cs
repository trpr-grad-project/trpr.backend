using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Modules.Users.Application.Dtos.Requests
{
    public class GuideUpgradeRequestDto
    {
        public string? Subject { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<DocumentDto> Documents { get; set; } = [];
    }
}
