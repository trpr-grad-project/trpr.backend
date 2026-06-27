using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Trips.Infrastructure.Options
{
    public class CategoryLimit
    {
        public string Category { get; init; } = "";

        public int MaxPlaces { get; init; }
    }
}