using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Domain.Entities
{
    public class Company : Entity
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<User> Guides { get; set; } = new List<User>();
        public static Company Create(Guid id, string identifier, string name, string logo, string description)
        {
            return new Company
            {
                Id = id,
                Name = name,
                Identifier = identifier,
                Logo = logo,
                Description = description,
                Guides = new List<User>()
            };
        }
    }
}
