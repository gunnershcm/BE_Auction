using API.Services.Interfaces;
using Domain.Constants.Enums;
using Domain.Models;
using Persistence.Repositories.Interfaces;

namespace API.Services.Implements
{
    public class HangFireService : IHangFireService
    {
        private readonly IRepositoryBase<Auction> _auctionRepository;

        public HangFireService(IRepositoryBase<Auction> auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task UpdateAuctionStatus()
        {
            var auctions = await _auctionRepository.ToListAsync(); 
            foreach (var auction in auctions)
            {
                DateTime currentTime = DateTime.Now;
                if (currentTime < auction.BiddingStartTime)
                {
                    auction.AuctionStatus = AuctionStatus.ComingUp; 
                }
                else if (currentTime >= auction.BiddingStartTime && currentTime <= auction.BiddingEndTime)
                {
                    auction.AuctionStatus = AuctionStatus.InProgress; 
                }
                else
                {
                    auction.AuctionStatus = AuctionStatus.Finished; 
                }
                await _auctionRepository.UpdateAsync(auction);
            }
        }

    }
}
