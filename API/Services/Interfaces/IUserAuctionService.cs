﻿using API.DTOs.Requests.UserAuctions;
using API.DTOs.Responses.UserAuctions;
using Domain.Models;

namespace API.Services.Implements
{
    public interface IUserAuctionService
    {
        Task JoinAuction(int userId, int auctionId);
        Task<List<GetUserAuctionResponse>> Get();
        Task<GetUserAuctionResponse> GetById(int id);
        Task<List<GetUserAuctionResponse>> GetAuctionByUser(int userId);
        Task<UserAuction> BiddingAmount(int userId, int auctionId, BiddingAmountRequest model);
    }
}
