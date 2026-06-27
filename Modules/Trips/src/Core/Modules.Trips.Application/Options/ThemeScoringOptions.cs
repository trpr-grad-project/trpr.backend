using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Trips.Infrastructure.Options
{
    public class ThemeScoringOptions
    {
        public StrategyOptions Strategy { get; init; } = new();

        public List<CategoryWeight> Categories { get; init; } = [];

        public List<TagWeight> Tags { get; init; } = [];
    }
}