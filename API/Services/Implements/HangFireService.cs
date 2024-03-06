using API.Services.Interfaces;
using Domain.Constants.Enums;
using Domain.Models;
using Persistence.Repositories.Interfaces;

namespace API.Services.Implements
{
    public class HangFireService : IHangFireService
    {
        private readonly IRepositoryBase<Auction> _auctionRepository;
        private readonly IRepositoryBase<UserAuction> _userAuctionRepository;
        private readonly IMailService _mailService;

        public HangFireService(IRepositoryBase<Auction> auctionRepository, IRepositoryBase<UserAuction> userAuctionRepository, IMailService mailService)
        {
            _auctionRepository = auctionRepository;
            _userAuctionRepository = userAuctionRepository;
            _mailService = mailService;
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

        public async Task SendMailAuction()
        {
            var currentTime = DateTime.Now;
            var auctionsStartingIn5Minutes = await _auctionRepository
                .FirstOrDefaultAsync(a => a.BiddingStartTime == currentTime.AddMinutes(5));

            //foreach (var auction in auctionsStartingIn5Minutes)
            //{
            //    var userAuctions = await _userAuctionRepository
            //        .WhereAsync(u => u.AuctionId == auction.Id);

            //    foreach (var userAuction in userAuctions)
            //    {
            //        await _mailService.SendUserAuctionNotification(userAuction.User.Username, userAuction.User.Email);
            //    }
            //}
            var userAuctions = await _userAuctionRepository.WhereAsync(u => u.AuctionId == auctionsStartingIn5Minutes.Id);
            foreach (var userauction in userAuctions)
            {
                await _mailService.SendUserAuctionNotification(userauction.User.Username, userauction.User.Email);
            }
            

        }

    }
}
