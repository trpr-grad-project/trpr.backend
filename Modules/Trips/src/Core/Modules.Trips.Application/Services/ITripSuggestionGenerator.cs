using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modules.Trips.Application.Dtos;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Services
{
    public interface ITripSuggestionGenerator
    {
        public Task<object> GenerateTrip(DateTime startDate, int numberOfDays, Theme theme, ICollection<PlaceDto> places);
    }

    public class AiTripSuggestionGenerator(
        IAiTripSuggestionService aiTripSuggestionService
    ) : ITripSuggestionGenerator
    {
        public Task<object> GenerateTrip(DateTime startDate, int numberOfDays, Theme theme, ICollection<PlaceDto> places)
        {
            return aiTripSuggestionService.GenerateTripPlan(startDate, theme, places, numberOfDays);
        }
    }

    public class AlgorithmicSuggestionGenerator : ITripSuggestionGenerator
    {
        public Task<object> GenerateTrip(DateTime startDate, int numberOfDays, Theme theme, ICollection<PlaceDto> places)
        {
            throw new NotImplementedException("Not Implemented Yet");
        }
    }
}