using Common.Application.Exceptions;
using Modules.Trips.Application.Abstractions;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;


namespace Modules.Trips.Application.Services
{
    public class BiddingService(RepositoryFactory repositoryFactory, IUnitOfWork unitOfWork)
    {
        public async Task PlaceBidAsync(Guid tripId, Guid guideId, double proposedPrice, string? proposalMessage, CancellationToken cancellationToken = default)
        {
            var tripRepository = repositoryFactory.Repository<Trip>();
            var userRepository = repositoryFactory.Repository<User>();
            var biddingRepository = repositoryFactory.Repository<TripBidding>();

            var trip = await tripRepository.GetFirstOrDefaultByFilter(x => x.Id == tripId) ?? throw new NotFoundException("Trip.NotFound");
            var guide = await userRepository.GetFirstOrDefaultByFilter(x => x.Id == guideId) ?? throw new NotFoundException("Guide.NotFound");
            var existingBid = await biddingRepository.GetFirstOrDefaultByFilter(b => b.TripId == tripId && b.GuideId == guideId);
            if (existingBid != null)
            {
                existingBid.ProposedPrice = proposedPrice;
                existingBid.ProposalMessage = proposalMessage;
                biddingRepository.Update(existingBid);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                return;
            }
            var bidding = TripBidding.Create(proposedPrice, proposalMessage, trip, guide);
            biddingRepository.Add(bidding);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}