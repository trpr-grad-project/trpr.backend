using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Trips.Application.Abstractions;
using Modules.Trips.Application.Dtos;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Helpers;
using Modules.Trips.Application.Mappers;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;
using Modules.Trips.Domain.ValueObjects;

namespace Modules.Trips.Application.Services
{
    public class BiddingService(RepositoryFactory repositoryFactory, IUnitOfWork unitOfWork)
    {
        public async Task PlaceBidAsync(CreateBiddingRequestDto dto, Guid guideId, CancellationToken cancellationToken = default)
        {
            var tripRepository = repositoryFactory.Repository<Trip>();
            var userRepository = repositoryFactory.Repository<User>();
            var biddingRepository = repositoryFactory.Repository<TripBidding>();

            var trip = await tripRepository.GetFirstOrDefaultByFilter(x => x.Id == dto.TripId)
                ?? throw new NotFoundException("Trip.NotFound", dto.TripId);

            if (trip.Status != TripStatus.Bidding)
                throw new ConflictException("Trip is not open for bidding.");

            if (trip.UserId == guideId)
                throw new ConflictException("Trip owner cannot place a bid on their own trip.");

            var guide = await userRepository.GetFirstOrDefaultByFilter(x => x.Id == guideId)
                ?? throw new NotFoundException("Guide.NotFound", guideId);

            var existingBid = await biddingRepository.GetFirstOrDefaultByFilter(
                b => b.TripId == dto.TripId && b.GuideId == guideId);

            if (existingBid != null)
            {
                existingBid.ProposalMessage = dto.ProposalMessage;
                existingBid.ProposedPrice = dto.ProposedPrice;
                existingBid.CreatedAtUTC = DateTime.UtcNow;
                biddingRepository.Update(existingBid);
            }
            else
            {
                var bidding = TripBidding.Create(dto.ProposedPrice, dto.ProposalMessage, trip, guide);
                biddingRepository.Add(bidding);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<BiddingCursorPageDto> GetTripBiddingsAsync(
            Guid tripId,
            Guid requestingUserId,
            GetTripBiddingsQueryDto query,
            CancellationToken cancellationToken = default)
        {
            var trip = await repositoryFactory.Repository<Trip>()
                .GetFirstOrDefaultByFilter(t => t.Id == tripId)
                ?? throw new NotFoundException("Trip.NotFound", tripId);

            if (trip.UserId != requestingUserId)
                throw new NotAuthorizedException("Trip.UnAuthorized");

            var biddingQuery = repositoryFactory.Repository<TripBidding>()
                .GetQueryable()
                .Include(b => b.Guide)
                .Where(b => b.TripId == tripId);

            biddingQuery = ApplyCursorFilter(biddingQuery, query.SortOrder, query.Cursor);
            biddingQuery = ApplyOrdering(biddingQuery, query.SortOrder);

            var pageSize = query.PageSize > 0 ? query.PageSize : 10;
            var items = await biddingQuery
                .Take(pageSize + 1)
                .ToListAsync(cancellationToken);

            var hasNextPage = items.Count > pageSize;
            var pageItems = hasNextPage ? items.Take(pageSize).ToList() : items;

            string? nextCursor = null;
            if (hasNextPage)
            {
                var last = pageItems.Last();
                nextCursor = query.SortOrder is BiddingSortOrder.Cheapest or BiddingSortOrder.MostExpensive
                    ? BiddingCursorHelper.EncodePriceCursor(last.ProposedPrice, last.CreatedAtUTC, last.Id)
                    : BiddingCursorHelper.EncodeTimeCursor(last.CreatedAtUTC, last.Id);
            }

            return new BiddingCursorPageDto
            {
                Items = pageItems.Select(b => b.ToBiddingResponseDto()).ToList(),
                NextCursor = nextCursor
            };
        }

        public async Task SelectBidAsync(Guid tripId, Guid biddingId, Guid requestingUserId, CancellationToken cancellationToken = default)
        {
            var trip = await repositoryFactory.Repository<Trip>()
                .GetFirstOrDefaultByFilter(t => t.Id == tripId)
                ?? throw new NotFoundException("Trip.NotFound", tripId);

            if (trip.UserId != requestingUserId)
                throw new NotAuthorizedException("UnAuthorized");

            var bidding = await repositoryFactory.Repository<TripBidding>()
                .GetFirstOrDefaultByFilter(b => b.Id == biddingId && b.TripId == tripId)
                ?? throw new NotFoundException("Bidding.NotFound", biddingId);

            try
            {
                trip.SelectGuide(bidding.GuideId);
            }
            catch (InvalidOperationException ex)
            {
                throw new ConflictException(ex.Message);
            }

            repositoryFactory.Repository<Trip>().Update(trip);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        #region Helpers

        private static IQueryable<TripBidding> ApplyCursorFilter(
            IQueryable<TripBidding> query,
            BiddingSortOrder sortOrder,
            string? cursor)
        {
            if (string.IsNullOrEmpty(cursor))
                return query;

            switch (sortOrder)
            {
                case BiddingSortOrder.Oldest:
                    {
                        var (createdAt, id) = BiddingCursorHelper.DecodeTimeCursor(cursor);
                        query = query.Where(b =>
                            b.CreatedAtUTC > createdAt ||
                            (b.CreatedAtUTC == createdAt && b.Id > id));
                        break;
                    }
                case BiddingSortOrder.Newest:
                    {
                        var (createdAt, id) = BiddingCursorHelper.DecodeTimeCursor(cursor);
                        query = query.Where(b =>
                            b.CreatedAtUTC < createdAt ||
                            (b.CreatedAtUTC == createdAt && b.Id < id));
                        break;
                    }
                case BiddingSortOrder.Cheapest:
                    {
                        var (price, createdAt, id) = BiddingCursorHelper.DecodePriceCursor(cursor);
                        query = query.Where(b =>
                            b.ProposedPrice > price ||
                            (b.ProposedPrice == price && b.CreatedAtUTC > createdAt) ||
                            (b.ProposedPrice == price && b.CreatedAtUTC == createdAt && b.Id > id));
                        break;
                    }
                case BiddingSortOrder.MostExpensive:
                    {
                        var (price, createdAt, id) = BiddingCursorHelper.DecodePriceCursor(cursor);
                        query = query.Where(b =>
                            b.ProposedPrice < price ||
                            (b.ProposedPrice == price && b.CreatedAtUTC < createdAt) ||
                            (b.ProposedPrice == price && b.CreatedAtUTC == createdAt && b.Id < id));
                        break;
                    }
            }

            return query;
        }

        private static IQueryable<TripBidding> ApplyOrdering(IQueryable<TripBidding> query, BiddingSortOrder sortOrder)
        {
            return sortOrder switch
            {
                BiddingSortOrder.Oldest => query.OrderBy(b => b.CreatedAtUTC).ThenBy(b => b.Id),
                BiddingSortOrder.Newest => query.OrderByDescending(b => b.CreatedAtUTC).ThenByDescending(b => b.Id),
                BiddingSortOrder.Cheapest => query.OrderBy(b => b.ProposedPrice).ThenBy(b => b.CreatedAtUTC).ThenBy(b => b.Id),
                BiddingSortOrder.MostExpensive => query.OrderByDescending(b => b.ProposedPrice).ThenByDescending(b => b.CreatedAtUTC).ThenByDescending(b => b.Id),
                _ => query.OrderBy(b => b.CreatedAtUTC).ThenBy(b => b.Id)
            };
        }

        #endregion
    }
}