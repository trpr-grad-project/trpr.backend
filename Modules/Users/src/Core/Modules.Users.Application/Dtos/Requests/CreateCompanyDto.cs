using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Modules.Users.Application.Helpers;

namespace Modules.Users.Application.Dtos.Requests
{
    public class CreateCompanyDto
    {
        public string Identifier { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        [JsonConverter(typeof(MaskedStringConverter))]
        public string Password { get; set; } = string.Empty;
    }
}
