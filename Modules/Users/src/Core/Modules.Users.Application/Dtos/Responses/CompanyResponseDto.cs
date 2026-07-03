using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Users.Application.Dtos.Responses
{
    public class CompanyResponseDto
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public List<UserResponseDto> Guides { get; set; } = new List<UserResponseDto>();
    }
}
