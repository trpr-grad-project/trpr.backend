using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Notifications.Application.Dtos.Requests
{
    public class PaginateRequestDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
