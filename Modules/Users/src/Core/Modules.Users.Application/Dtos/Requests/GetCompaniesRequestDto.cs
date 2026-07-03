using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Users.Application.Dtos.Requests
{
    public class GetCompaniesRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Identifier { get; set; }
        public string? CompanyName { get; set; }
    }
}
