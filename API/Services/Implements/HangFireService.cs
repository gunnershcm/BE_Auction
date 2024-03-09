﻿using API.DTOs.Responses.UserAuctions;
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
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public HangFireService(IRepositoryBase<Auction> auctionRepository, IRepositoryBase<UserAuction> userAuctionRepository, 
            IMailService mailService, IMapper mapper)
        {
            _auctionRepository = auctionRepository;
            _userAuctionRepository = userAuctionRepository;
            _mailService = mailService;
            _mapper = mapper;
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
            DateTime currentTime = DateTime.Now;
            var auctionsStartingIn5Minutes = await _auctionRepository
                .WhereAsync(a => a.BiddingStartTime <= currentTime.AddMinutes(5) && a.BiddingStartTime > currentTime);

            foreach (var auction in auctionsStartingIn5Minutes)
            {
                var userAuctions = await _userAuctionRepository
                    .WhereAsync(u => u.AuctionId == auction.Id);              
                
                foreach (var userAuction in userAuctions)
                {
                    if (userAuction.User != null)
                    {
                        await _mailService.SendUserAuctionNotification(userAuction.User.Username, userAuction.User.Email);
                    }
                    else
                    {
                        throw new Exception("User is not exist");
                    }
                }
            }
        }


    }
}