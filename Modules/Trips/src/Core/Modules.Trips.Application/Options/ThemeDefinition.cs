using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Trips.Infrastructure.Options
{
    public class ThemeDefinition
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public GeneratorOptions Generator { get; init; } = new();

        public ThemeScoringOptions Scoring { get; init; } = new();
    }
}