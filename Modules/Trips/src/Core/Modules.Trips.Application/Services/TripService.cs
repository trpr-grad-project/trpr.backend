using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Application;
using Common.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Modules.Trips.Application.Abstractions;
using Modules.Trips.Application.Dtos;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Mappers;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Services
{
    public class TripService(IUnitOfWork unitOfWork,
        RepositoryFactory repositoryFactory, 
        IMapper<ICollection<IFormFile>,Task<ICollection<string>>> imageMapper,
        IMapper<ICollection<DayDto>, Task<ICollection<ICollection<Place>>>> daysMapper,
        IMapper<Trip, CreateTripResponseDto> tripMapper)
    {
        public async Task<CreateTripResponseDto> CreateTrip(CreateTripRequestDto dto, Guid userId, CancellationToken cancellationToken)
        {
            var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(User => User.Id == userId)
                ?? throw new NotFoundException("User.NotFound", userId);
            var paths = await imageMapper.Map(dto.Images);
            var segments = await daysMapper.Map(dto.Segments);
            var trip = Trip.Create(userId, dto.ThemeId, dto.Title, dto.Description, dto.Price, paths, 
                            dto.TripVisibility, segments, dto.MaxParticipantsCount, dto.guideId, 
                            dto.Segments.Select(s => s.Duration).ToList(), user);
            repositoryFactory.Repository<Trip>().Add(trip);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return tripMapper.Map(trip);
        }
    }
}
