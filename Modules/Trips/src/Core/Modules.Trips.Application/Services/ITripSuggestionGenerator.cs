using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modules.Trips.Application.Dtos;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Services
{
    public interface ITripSuggestionGenerator
    {
        public Task<List<DayDto>> GenerateTrip(int numberOfDays, Theme theme, ICollection<Place> places);
    }

    public class AiTripSuggestionGenerator : ITripSuggestionGenerator
    {
        public Task<List<DayDto>> GenerateTrip(int numberOfDays, Theme theme, ICollection<Place> places)
        {
            throw new NotImplementedException("Not Implemented Yet");
        }
    }

    public class AlgorithmicSuggestionGenerator : ITripSuggestionGenerator
    {
        public Task<List<DayDto>> GenerateTrip(int numberOfDays, Theme theme, ICollection<Place> places)
        {
            throw new NotImplementedException("Not Implemented Yet");
        }
    }
}