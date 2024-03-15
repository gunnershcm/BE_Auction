using API.DTOs.Requests.UserAuctions;
using API.DTOs.Responses.AuctionHistories;
using Domain.Models;

namespace API.Services.Interfaces
{
    public interface IAuctionHistoryService
    {
        Task<List<GetAuctionHistoryResponse>> Get();
        Task<GetAuctionHistoryResponse> GetById(int id);
        Task<List<GetHistoryByAuctionResponse>> GetHistoryByAuction(int auctionId);
        Task<List<GetHistoryByUserResponse>> GetHistoryByUser(int userId);
        Task<AuctionHistory> CreateAuctionHistory(int userId, int auctionId, BiddingHistoryRequest model);
    }
}
