using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Trips.Application.Abstractions;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Mappers;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;
using Modules.Trips.Application.Helpers;

namespace Modules.Trips.Application.Services;

public class PlaceService(IUnitOfWork unitOfWork, RepositoryFactory repositoryFactory)
{
    public async Task<PlaceFormDataDto> GetPlaceFormDataAsync(CancellationToken cancellationToken = default)
    {
        var categories = await repositoryFactory.Repository<Category>().GetQueryable()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync(cancellationToken);

        var governorates = await repositoryFactory.Repository<Governorate>().GetQueryable()
            .Select(g => new GovernorateDto
            {
                Id = g.Id,
                Name = g.Name
            }).ToListAsync(cancellationToken);

        var tags = await repositoryFactory.Repository<Tag>().GetQueryable()
            .Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToListAsync(cancellationToken);

        return new PlaceFormDataDto
        {
            Categories = categories,
            Governorates = governorates,
            Tags = tags
        };
    }
    public async Task<PlaceDto> UpdatePlaceAsync(Guid? userId, int id, UpdatePlaceRequestDto dto, CancellationToken cancellationToken = default)
    {
        var place = await repositoryFactory.Repository<Place>()
            .GetFirstOrDefaultByFilter(p => p.Id == id && p.UserId == userId,
                x => x
                    .Include(x => x.Governorate)
                    .Include(x => x.Category)
                    .Include(x => x.PlaceTags).ThenInclude(x => x.Tag))
            ?? throw new NotFoundException("Place.NotFound");

        if (dto.CategoryId != null)
        {
            var category = await repositoryFactory
                .Repository<Category>()
                .GetFirstOrDefaultByFilter(c => c.Id == dto.CategoryId)
                ?? throw new NotFoundException("Category.NotFound");
            place.CategoryId = dto.CategoryId.Value;
        }

        if (dto.GovernorateId != null)
        {
            var governorate = await repositoryFactory
                .Repository<Governorate>()
                .GetFirstOrDefaultByFilter(g => g.Id == dto.GovernorateId)
                ?? throw new NotFoundException("Governorate.NotFound");
            place.GovernorateId = dto.GovernorateId.Value;
        }

        if (dto.TagIds != null)
        {
            foreach (var tagId in dto.TagIds)
                _ = await repositoryFactory.Repository<Tag>().GetFirstOrDefaultByFilter(t => t.Id == tagId)
                        ?? throw new NotFoundException("Tag.NotFound");

            place.PlaceTags.Clear();
            foreach (var tagId in dto.TagIds)
            {
                var placeTag = new PlaceTag { PlaceId = place.Id, TagId = tagId };
                place.PlaceTags.Add(placeTag);
            }
        }

        if (dto.Title != null)
            place.Title = dto.Title;
        if (dto.Description != null)
            place.Description = dto.Description;
        if (dto.Longitude != null && dto.Latitude != null)
            place.Location = Place.Create(
                null,
                "dummy",
                "dummy",
                1,
                1,
                dto.Longitude.Value,
                dto.Latitude.Value).Location;

        repositoryFactory.Repository<Place>().Update(place);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        place = await repositoryFactory
            .Repository<Place>()
            .GetFirstOrDefaultByFilter(p => p.Id == place.Id,
                x => x
                    .Include(x => x.Governorate)
                    .Include(x => x.Category)
                    .Include(x => x.PlaceTags).ThenInclude(x => x.Tag))
            ?? throw new NotFoundException("Place.NotFound");

        return place.ToPlaceDto();
    }
    public async Task<PlaceDto> CreatePlaceAsync(Guid? userId, CreatePlaceRequestDto dto, CancellationToken cancellationToken = default)
    {
        User? user = userId != null ? (await repositoryFactory
            .Repository<User>()
            .GetFirstOrDefaultByFilter(x => x.Id == userId)
            ?? throw new NotFoundException("User.NotFound")) : null;

        var category = await repositoryFactory
            .Repository<Category>()
            .GetFirstOrDefaultByFilter(c => c.Id == dto.CategoryId)
            ?? throw new NotFoundException("Category.NotFound");

        var governorate = await repositoryFactory
            .Repository<Governorate>()
            .GetFirstOrDefaultByFilter(g => g.Id == dto.GovernorateId)
            ?? throw new NotFoundException("Governorate.NotFound");

        foreach (var tagId in dto.TagIds)
            _ = await repositoryFactory.Repository<Tag>().GetFirstOrDefaultByFilter(t => t.Id == tagId)
                    ?? throw new NotFoundException("Tag.NotFound");

        var place = Place.Create(
            user,
            dto.Title,
            dto.Description,
            dto.CategoryId,
            dto.GovernorateId,
            dto.Longitude,
            dto.Latitude);

        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            repositoryFactory.Repository<Place>().Add(place);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            foreach (var tagId in dto.TagIds)
            {
                var placeTag = new PlaceTag { PlaceId = place.Id, TagId = tagId };
                place.PlaceTags.Add(placeTag);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }

        place = await repositoryFactory
            .Repository<Place>()
            .GetFirstOrDefaultByFilter(p => p.Id == place.Id,
                x => x
                    .Include(x => x.Governorate)
                    .Include(x => x.Category)
                    .Include(x => x.PlaceTags).ThenInclude(x => x.Tag))
            ?? throw new NotFoundException("Place.NotFound");

        return place.ToPlaceDto();
    }
    public async Task<CursorPageDto<PlaceDto, int?>> GetPlacesAsync(Guid? userId, GetPlacesQueryDto query, CancellationToken cancellationToken = default)
    {
        var queryable = repositoryFactory
            .Repository<Place>()
            .GetQueryable();

        // filter by governorate
        if (query.GovernorateId != null)
            queryable = queryable.Where(x => x.GovernorateId == query.GovernorateId);
        // filter by location
        if (query.Longitude != null && query.Latitude != null && query.RadiusInMeters != null)
        {
            var point = PointUtils
                .PointFromCoordinates(
                    query.Longitude.Value,
                    query.Latitude.Value);
            queryable = queryable.Where(x => x.Location.Distance(point) <= query.RadiusInMeters.Value);
        }
        // filter by place name 
        if (!string.IsNullOrWhiteSpace(query.Title))
            queryable = queryable.Where(x => x.Title.StartsWith(query.Title));
        // oder by id 
        queryable = queryable
            .OrderByDescending(x => x.Id);
        // cursor pagination
        if (query.LastPlaceId != null)
            queryable = queryable.Where(x => x.Id < query.LastPlaceId);
        // filter by userId
        queryable = queryable.Where(x => x.UserId == userId);
        // includes and page size
        queryable = queryable
            .Include(x => x.Category)
            .Include(x => x.Governorate)
            .Include(x => x.PlaceTags)
                .ThenInclude(x => x.Tag);

        if (query.PageSize != null)
            queryable = queryable.Take(query.PageSize.Value + 1);

        var places = await queryable
            .Select(x => x.ToPlaceDto())
            .ToListAsync(cancellationToken);
        // check if it has a next page
        var hasNextPage = places.Count > query.PageSize;

        if (hasNextPage)
        {
            // remove the last elemnt to use it in the next cursor search
            places.RemoveAt(places.Count - 1);
        }

        int? nextCursor = places.LastOrDefault()?.Id;

        return new CursorPageDto<PlaceDto, int?>
        {
            Items = places,
            NextCursor = nextCursor
        };
    }

    public async Task<ICollection<Place>> GetPlacesAsync(ICollection<int> placeIds)
    {
        ICollection<Place> places = [];
        foreach (var placeId in placeIds)
        {
            var place = await
                repositoryFactory
                .Repository<Place>()
                .GetFirstOrDefaultByFilter(
                    p => p.Id == placeId,
                    includes: x => x.Include(p => p.Governorate)
                        .Include(p => p.Category)
                        .Include(p => p.PlaceTags).ThenInclude(p => p.Tag))
                ?? throw new NotFoundException("Place.NotFound");
            places.Add(place);
        }
        return places;
    }
}
