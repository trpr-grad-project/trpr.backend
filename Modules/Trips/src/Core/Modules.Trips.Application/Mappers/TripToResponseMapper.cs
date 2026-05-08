using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Application;
using Common.Application.Buckets;
using Modules.Trips.Application.Dtos;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Domain.Entities;


namespace Modules.Trips.Application.Mappers
{
    public class TripToResponseMapper(IMapper<ICollection<string>, ICollection<string>> urlMapper) : IMapper<Trip, CreateTripResponseDto>
    {
        public CreateTripResponseDto Map(Trip source)
        {
            ICollection<string> imagePaths = urlMapper.Map(source.Images);
            CreateTripResponseDto responseDto = new CreateTripResponseDto
            {
                CreatedByUser = source.UserId,
                GuideId = source.GuideId,
                ThemeId = source.ThemeId,
                Title = source.Title,
                Description = source.Description,
                Price = source.Price,
                ExpectedDuration = source.ExpectedDuration,
                ImagesUrls = imagePaths,
                TripVisibility = source.TripVisibility,
                Segments = source.Segments
                    .SelectMany(s => s.Places)
                    .Select(x => x.ToPlaceDto())
                    .ToList(),
                MaxParticipantsCount = source.MaxParticipantsCount,
            };
            return responseDto;
        }
    }
}
