using API.DTOs.Responses.UserAuctions;
using API.Services.Interfaces;
using AutoMapper;
using Domain.Constants.Enums;
using Domain.Models;
using Persistence.Repositories.Interfaces;

namespace API.Services.Implements
{
    public class HangFireService : IHangFireService
    {
        private readonly IRepositoryBase<Auction> _auctionRepository;
        private readonly IRepositoryBase<UserAuction> _userAuctionRepository;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly IRepositoryBase<Transaction> _transactionRepository;

        public HangFireService(IRepositoryBase<Auction> auctionRepository, IRepositoryBase<UserAuction> userAuctionRepository,
            IMailService mailService, IUserService userService, IRepositoryBase<Transaction> transactionRepository)
        {
            _auctionRepository = auctionRepository;
            _userAuctionRepository = userAuctionRepository;
            _mailService = mailService;
            _userService = userService;
            _transactionRepository = transactionRepository;
        }

        public async Task UpdateAuctionStatus()
        {
            var auctions = await _auctionRepository.ToListAsync();
            
            foreach (var auction in auctions)
            {
                var target = await _transactionRepository.FirstOrDefaultAsync(u => u.AuctionId == auction.Id);
                DateTime currentTime = DateTime.Now;
                if (currentTime < auction.BiddingStartTime)
                {
                    auction.AuctionStatus = AuctionStatus.ComingUp;
                }
                else if (currentTime >= auction.BiddingStartTime && currentTime <= auction.BiddingEndTime)
                {
                    auction.AuctionStatus = AuctionStatus.InProgress;
                }
                else if (currentTime > auction.BiddingEndTime)
                {
                    if (target != null)
                    {
                        auction.AuctionStatus = AuctionStatus.Succeeded;
                    }
                    else
                    {
                        if(currentTime >= auction.BiddingEndTime.AddMinutes(10))
                        {
                            auction.AuctionStatus = AuctionStatus.Failed;
                        }
                        else
                        {
                            auction.AuctionStatus = AuctionStatus.Finished;
                        }
                    }
                }
                await _auctionRepository.UpdateAsync(auction);
            }
        }


        public async Task SendMailAuction()
        {
            DateTime currentTime = DateTime.Now;
            var auctionsStartingIn2Minutes = await _auctionRepository
                .WhereAsync(a => a.BiddingStartTime <= currentTime.AddMinutes(2) && a.BiddingStartTime > currentTime);

            foreach (var auction in auctionsStartingIn2Minutes)
            {
                var userAuctions = await _userAuctionRepository
                    .WhereAsync(u => u.AuctionId == auction.Id);
                foreach (var userAuction in userAuctions)
                {
                    var user = await _userService.GetById(userAuction.UserId);
                    await _mailService.SendUserAuctionNotification(user.Username, user.Email);
                }
            }
        }


    }
}
