using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Modules.Users.Application.Dtos.Requests
{
    public class GuideUpgradeRequestDto
    {
        public Guid Id { get; set; }
        public ICollection<DocumentDto> Documents { get; set; } = new List<DocumentDto>();
    }
}
