using System;
using System.Collections.Generic;
using System.Text;

namespace Modules.Trips.Domain.Entities
{
    [Flags]
    public enum UserRole
    {
        User = 1, // 001     
        Guide = 2, // 010     
        Admin = 4,  // 100 
        Company = 8,
    }
}
