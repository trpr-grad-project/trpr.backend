using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Application;
using Microsoft.AspNetCore.Http;
using Modules.Trips.Application.Dtos;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Mappers
{
    public class DaysMapper(RepositoryFactory repositoryFactory) : IMapper<ICollection<DayDto>, Task<ICollection<ICollection<Place>>>>
    {
        public async Task<ICollection<ICollection<Place>>> Map(ICollection<DayDto> source)
        {
            ICollection<ICollection<Place>> places = [];
            foreach (var day in source)
            {
                ICollection<Place> dayPlaces = [];
                foreach (var placeId in day.PlacesIds)
                {
                    var place = await repositoryFactory.Repository<Place>().GetFirstOrDefaultByFilter(p => p.Id == placeId);
                    if (place is not null)
                        dayPlaces.Add(place);
                }
                places.Add(dayPlaces);
            }
            return places;
        }
    }
}
