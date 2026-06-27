using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Trips.Infrastructure.Options
{
    public class ThemeOptions
    {
        public StrategyOptions Defaults { get; init; } = new();

        public List<ThemeDefinition> Themes { get; init; } = [];
    }
}