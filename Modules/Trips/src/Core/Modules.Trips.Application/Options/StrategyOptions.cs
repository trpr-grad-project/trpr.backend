using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Trips.Infrastructure.Options
{
    public class StrategyOptions
    {
        public double ThemeWeight { get; init; }

        public double RatingWeight { get; init; }

        public double CategoryWeight { get; init; }

        public double TravelPenaltyWeight { get; init; }

        public double TagDecayFactor { get; init; }

        public double AverageDrivingSpeedKmPerHour { get; init; }
    }
}