using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Trips.Infrastructure.Options
{
    public class GeneratorOptions
    {
        public List<CategoryLimit> CategoryLimits { get; init; } = [];
    }
}